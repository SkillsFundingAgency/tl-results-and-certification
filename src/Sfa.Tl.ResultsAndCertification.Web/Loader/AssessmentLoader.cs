﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
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

        public async Task<T> GetAssessmentDetailsAsync<T>(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var learnerDetails = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId, status);
            var assessmentSeries = await _internalApiClient.GetAssessmentSeriesAsync();

            if (learnerDetails == null || assessmentSeries == null || !assessmentSeries.Any())
                return _mapper.Map<T>(null);

            var learnerAssessmentDetails = _mapper.Map<T>(learnerDetails, opt =>
            {
                opt.Items["currentCoreAssessmentSeriesId"] = CommonHelper.GetValidAssessmentSeries(assessmentSeries, learnerDetails.Pathway.AcademicYear, learnerDetails.Pathway.StartYear, ComponentType.Core)?.FirstOrDefault()?.Id ?? 0;
                opt.Items["currentSpecialismAssessmentSeriesId"] = CommonHelper.GetValidAssessmentSeries(assessmentSeries, learnerDetails.Pathway.AcademicYear, learnerDetails.Pathway.StartYear, ComponentType.Specialism)?.FirstOrDefault()?.Id ?? 0;
                opt.Items["coreSeriesName"] = CommonHelper.GetNextAvailableAssessmentSeries(assessmentSeries, learnerDetails.Pathway.AcademicYear, learnerDetails.Pathway.StartYear, ComponentType.Core)?.Name;
                opt.Items["specialismSeriesName"] = CommonHelper.GetNextAvailableAssessmentSeries(assessmentSeries, learnerDetails.Pathway.AcademicYear, learnerDetails.Pathway.StartYear, ComponentType.Specialism)?.Name;
            });
            return learnerAssessmentDetails;
        }

        public async Task<T> GetAddAssessmentEntryAsync<T>(long aoUkprn, int profileId, ComponentType componentType, string componentIds = null)
        {
            var learnerDetails = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId);
            if (learnerDetails == null || (componentType == ComponentType.Specialism && componentIds == null))
                return _mapper.Map<T>(null);

            var ids = componentType == ComponentType.Core ? learnerDetails.Pathway.Id.ToString() : componentIds;

            var availableSeries = await _internalApiClient.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, componentType, ids);
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

        public async Task<RemoveSpecialismAssessmentEntryViewModel> GetRemoveSpecialismAssessmentEntriesAsync(long aoUkprn, int profileId, string specialismAssessmentIds)
        {
            // Ensure the input specialismAssessmentIds contains numbers. 
            var requestedAssessmentIds = specialismAssessmentIds?.Split(Constants.PipeSeperator)?.ToList();
            if (requestedAssessmentIds == null || !requestedAssessmentIds.All(x => x.IsInt()))
                return null;

            // Ensure learner details are found
            var learnerDetails = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId);
            if (learnerDetails == null || learnerDetails.Pathway == null || !learnerDetails.Pathway.Specialisms.Any())
                return null;

            // Ensure all requested specialisms assessmentId's are valid
            var splAssessmentsFoundInRegistration = learnerDetails.Pathway.Specialisms.Where(x => x.Assessments.Any(a => requestedAssessmentIds.Contains(a.Id.ToString())));
            if (requestedAssessmentIds.Count() != splAssessmentsFoundInRegistration.Count())
                return null;

            // Ensure all requested entries are currently active
            var assessmentEntryDetails = await _internalApiClient.GetActiveSpecialismAssessmentEntriesAsync(aoUkprn, specialismAssessmentIds);
            if (assessmentEntryDetails == null || requestedAssessmentIds.Count() != assessmentEntryDetails.Count() || assessmentEntryDetails.GroupBy(x => x.AssessmentSeriesName).Count() != 1)
                return null;

            var removeAsessmentEntryViewModel = _mapper.Map<RemoveSpecialismAssessmentEntryViewModel>(learnerDetails, opt => { opt.Items["currentSpecialismAssessmentSeriesId"] = assessmentEntryDetails.FirstOrDefault().AssessmentSeriesId; });
            removeAsessmentEntryViewModel.AssessmentSeriesName = assessmentEntryDetails.FirstOrDefault().AssessmentSeriesName.ToLowerInvariant();
            removeAsessmentEntryViewModel.SpecialismAssessmentIds = specialismAssessmentIds;
            return removeAsessmentEntryViewModel;
        }

        public async Task<bool> RemoveSpecialismAssessmentEntryAsync(long aoUkprn, RemoveSpecialismAssessmentEntryViewModel viewModel)
        {
            var request = _mapper.Map<RemoveAssessmentEntryRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.RemoveAssessmentEntryAsync(request);
        }

        public async Task<IList<DataExportResponse>> GenerateAssessmentsExportAsync(long aoUkprn, string requestedBy)
        {
            return await _internalApiClient.GenerateDataExportAsync(aoUkprn, DataExportType.Assessments, requestedBy);
        }

        public async Task<Stream> GetAssessmentsDataFileAsync(long aoUkprn, Guid blobUniqueReference, ComponentType componentType)
        {
            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.DataExports.ToString(),
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                SourceFilePath = $"{aoUkprn}/{DataExportType.Assessments}/{componentType}"
            });

            if (fileStream == null)
            {
                var blobReadError = $"No FileStream found to download assessments data. Method: GetAssessmentsDataFileAsync(ContainerName: {DocumentType.Assessments}, BlobFileName = {blobUniqueReference}, SourceFilePath = {aoUkprn}/{DataExportType.Assessments})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }
            return fileStream;
        }
    }
}
