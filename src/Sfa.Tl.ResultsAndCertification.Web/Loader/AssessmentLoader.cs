using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AssessmentLoader : IAssessmentLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<AssessmentLoader> _logger;

        public AssessmentLoader(IMapper mapper, ILogger<AssessmentLoader> logger, IResultsAndCertificationInternalApiClient internalApiClient, IBlobStorageService blobStorageService)
        {
            _mapper = mapper;
            _logger = logger;
            _internalApiClient = internalApiClient;
            _blobStorageService = blobStorageService;
        }

        public async Task<UploadAssessmentsResponseViewModel> ProcessBulkAssessmentsAsync(UploadAssessmentsRequestViewModel viewModel)
        {
            var bulkAssessmentRequest = _mapper.Map<BulkProcessRequest>(viewModel);

            using (var fileStream = viewModel.File.OpenReadStream())
            {
                await _blobStorageService.UploadFileAsync(new BlobStorageData
                {
                    ContainerName = bulkAssessmentRequest.DocumentType.ToString(),
                    BlobFileName = bulkAssessmentRequest.BlobFileName,
                    SourceFilePath = $"{bulkAssessmentRequest.AoUkprn}/{BulkProcessStatus.Processing}",
                    FileStream = fileStream,
                    UserName = bulkAssessmentRequest.PerformedBy
                });
            }

            var bulkAssessmentResponse = await _internalApiClient.ProcessBulkAssessmentsAsync(bulkAssessmentRequest);
            return _mapper.Map<UploadAssessmentsResponseViewModel>(bulkAssessmentResponse);
        }

        public async Task<Stream> GetAssessmentValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference)
        {
            var documentInfo = await _internalApiClient.GetDocumentUploadHistoryDetailsAsync(aoUkprn, blobUniqueReference);

            if (documentInfo != null && documentInfo.Status == (int)DocumentUploadStatus.Failed)
            {
                var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
                {
                    ContainerName = DocumentType.Assessments.ToString(),
                    BlobFileName = documentInfo.BlobFileName,
                    SourceFilePath = $"{aoUkprn}/{BulkProcessStatus.ValidationErrors}"
                });

                if (fileStream == null)
                {
                    var blobReadError = $"No FileStream found to download assessment validation errors. Method: DownloadFileAsync(ContainerName: {DocumentType.Assessments}, BlobFileName = {documentInfo.BlobFileName}, SourceFilePath = {aoUkprn}/{BulkProcessStatus.ValidationErrors})";
                    _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
                }
                return fileStream;
            }
            else
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No DocumentUploadHistoryDetails found or the request is not valid. Method: GetDocumentUploadHistoryDetailsAsync(AoUkprn: {aoUkprn}, BlobUniqueReference = {blobUniqueReference})");
                return null;
            }
        }

        public async Task<UlnAssessmentsNotFoundViewModel> FindUlnAssessmentsAsync(long aoUkprn, long Uln)
        {
            var response = await _internalApiClient.FindUlnAsync(aoUkprn, Uln);
            return _mapper.Map<UlnAssessmentsNotFoundViewModel>(response);
        }

        public async Task<T> GetAssessmentDetailsAsync<T>(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var learnerDetails = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId, status);
            var assessmentSeries = await _internalApiClient.GetAssessmentSeriesAsync();

            if (learnerDetails == null || assessmentSeries == null || !assessmentSeries.Any())
                return _mapper.Map<T>(null);

            var learnerAssessmentDetails = _mapper.Map<T>(learnerDetails, opt =>
            {
                opt.Items["currentCoreAssessmentSeriesId"] = GetValidAssessmentSeries(assessmentSeries, learnerDetails.Pathway.AcademicYear, ComponentType.Core)?.FirstOrDefault()?.Id ?? 0;
                opt.Items["currentSpecialismAssessmentSeriesId"] = GetValidAssessmentSeries(assessmentSeries, learnerDetails.Pathway.AcademicYear, ComponentType.Specialism)?.FirstOrDefault()?.Id ?? 0;
                opt.Items["coreSeriesName"] = GetNextAvailableAssessmentSeries(assessmentSeries, learnerDetails.Pathway.AcademicYear, ComponentType.Core)?.Name;
                opt.Items["specialismSeriesName"] = GetNextAvailableAssessmentSeries(assessmentSeries, learnerDetails.Pathway.AcademicYear, ComponentType.Specialism)?.Name;
            });
            return learnerAssessmentDetails;
        }

        public async Task<T> GetAddAssessmentEntryAsync<T>(long aoUkprn, int profileId, ComponentType componentType, string componentLarIds = null)
        {
            var learnerDetails = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId);
            if (learnerDetails == null || (componentType == ComponentType.Specialism && componentLarIds == null))
                return _mapper.Map<T>(null);

            var componentLarIdValues = componentLarIds?.Split(Constants.PipeSeperator).ToList();
            var componentIds = componentType == ComponentType.Core ? learnerDetails.Pathway.Id.ToString() :
                string.Join(Constants.PipeSeperator, learnerDetails.Pathway.Specialisms.Where(x => componentLarIdValues.Contains(x.LarId, StringComparer.InvariantCultureIgnoreCase)).Select(x => x.Id));

            var availableSeries = await _internalApiClient.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, componentType, componentIds);
            if (availableSeries == null)
                return _mapper.Map<T>(null);

            var result = _mapper.Map<T>(learnerDetails, opt =>
            {
                opt.Items["currentSpecialismAssessmentSeriesId"] = availableSeries.AssessmentSeriesId;
            });
            _mapper.Map(availableSeries, result);

            return result;
        }

        public async Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(long aoUkprn, AddAssessmentEntryViewModel viewModel)
        {
            var request = _mapper.Map<AddAssessmentEntryRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.AddAssessmentEntryAsync(request);
        }

        public async Task<AddAssessmentEntryResponse> AddSpecialismAssessmentEntryAsync(long aoUkprn, AddSpecialismAssessmentEntryViewModel viewModel)
        {
            var request = _mapper.Map<AddAssessmentEntryRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.AddAssessmentEntryAsync(request);
        }

        public async Task<AssessmentEntryDetailsViewModel> GetActiveAssessmentEntryDetailsAsync(long aoUkprn, int assessmentId, ComponentType componentType)
        {
            var assessmentEntryDetails = await _internalApiClient.GetActiveAssessmentEntryDetailsAsync(aoUkprn, assessmentId, componentType);
            if (assessmentEntryDetails == null) return null;

            var learnerDetails = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, assessmentEntryDetails.ProfileId);
            if (learnerDetails == null) return null;

            var result = _mapper.Map<AssessmentEntryDetailsViewModel>(learnerDetails);
            return _mapper.Map(assessmentEntryDetails, result);
        }

        public async Task<bool> RemoveAssessmentEntryAsync(long aoUkprn, AssessmentEntryDetailsViewModel viewModel)
        {
            var request = _mapper.Map<RemoveAssessmentEntryRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.RemoveAssessmentEntryAsync(request);
        }

        public async Task<RemoveSpecialismAssessmentEntryViewModel> GetRemoveSpecialismAssessmentEntriesAsync(long aoUkprn, int profileId, string specialismLarId)
        {
            // Ensure learner details are found
            var learnerDetails = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId);
            if (learnerDetails == null || learnerDetails.Pathway == null || !learnerDetails.Pathway.Specialisms.Any())
                return null;

            // Ensure all requested specialisms LarIds are valid
            var requestedLarIds = specialismLarId?.Split(Constants.PipeSeperator).ToList();
            var specialismIdsFoundInRegistration = learnerDetails.Pathway.Specialisms.Where(x => requestedLarIds.Contains(x.LarId, StringComparer.InvariantCultureIgnoreCase)).Select(x => x.Id);
            if (requestedLarIds.Count() != specialismIdsFoundInRegistration.Count())
                return null;

            // Ensure all requested entries are currently active
            var assessmentEntryDetails = await _internalApiClient.GetActiveSpecialismAssessmentEntriesAsync(aoUkprn, string.Join(Constants.PipeSeperator, specialismIdsFoundInRegistration));
            //if (assessmentEntryDetails == null || requestedLarIds.Count() != assessmentEntryDetails.Count())
            //    return null;

            return _mapper.Map<RemoveSpecialismAssessmentEntryViewModel>(learnerDetails);
        }

        public async Task<bool> RemoveSpecialismAssessmentEntryAsync(long aoUkprn, RemoveSpecialismAssessmentEntryViewModel viewModel)
        {
            var request = _mapper.Map<RemoveAssessmentEntryRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.RemoveAssessmentEntryAsync(request);
        }

        #region Private methods

        private IList<AssessmentSeriesDetails> GetValidAssessmentSeries(IList<AssessmentSeriesDetails> assessmentSeries, int academicYear, ComponentType componentType)
        {
            var currentDate = DateTime.UtcNow.Date;
            var startInYear = componentType == ComponentType.Specialism ? Constants.SpecialismAssessmentStartInYears : Constants.CoreAssessmentStartInYears;

            var series = assessmentSeries?.Where(s => s.ComponentType == componentType && s.Year > academicYear + startInYear &&
                                        s.Year <= academicYear + Constants.AssessmentEndInYears &&
                                        currentDate >= s.StartDate && currentDate <= s.EndDate)?.OrderBy(a => a.Id)?.ToList();

            return series;
        }

        private AssessmentSeriesDetails GetNextAvailableAssessmentSeries(IList<AssessmentSeriesDetails> assessmentSeries, int academicYear, ComponentType componentType)
        {
            var startInYear = componentType == ComponentType.Specialism ? Constants.SpecialismAssessmentStartInYears : Constants.CoreAssessmentStartInYears;
            var series = assessmentSeries?.OrderBy(a => a.Id)?.FirstOrDefault(s => s.ComponentType == componentType && s.Year > academicYear + startInYear &&
                                        s.Year <= academicYear + Constants.AssessmentEndInYears && DateTime.UtcNow.Date <= s.EndDate);
            return series;
        }

        #endregion
    }
}
