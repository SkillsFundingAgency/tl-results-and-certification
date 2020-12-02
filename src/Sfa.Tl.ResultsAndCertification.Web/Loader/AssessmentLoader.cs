using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Assessment;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.IO;
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

        public async Task<AssessmentDetailsViewModel> GetAssessmentDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var response = await _internalApiClient.GetAssessmentDetailsAsync(aoUkprn, profileId, status);
            return _mapper.Map<AssessmentDetailsViewModel>(response);
        }

        public async Task<AddAssessmentSeriesViewModel> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, AssessmentEntryType assessmentEntryType)
        {
            var response = await _internalApiClient.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, assessmentEntryType);
            return _mapper.Map<AddAssessmentSeriesViewModel>(response);
        }

        public async Task<AddAssessmentSeriesResponse> AddAssessmentSeriesAsync(long aoUkprn, AddAssessmentSeriesViewModel viewModel)
        {
            var request = _mapper.Map<AddAssessmentSeriesRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.AddAssessmentSeriesAsync(request);
        }
    }
}
