using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class ResultLoader : IResultLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<ResultLoader> _logger;

        public ResultLoader(IMapper mapper, ILogger<ResultLoader> logger, IResultsAndCertificationInternalApiClient internalApiClient, IBlobStorageService blobStorageService)
        {
            _mapper = mapper;
            _logger = logger;
            _internalApiClient = internalApiClient;
            _blobStorageService = blobStorageService;
        }

        public async Task<UploadResultsResponseViewModel> ProcessBulkResultsAsync(UploadResultsRequestViewModel viewModel)
        {
            var bulkResultRequest = _mapper.Map<BulkProcessRequest>(viewModel);

            using (var fileStream = viewModel.File.OpenReadStream())
            {
                await _blobStorageService.UploadFileAsync(new BlobStorageData
                {
                    ContainerName = bulkResultRequest.DocumentType.ToString(),
                    BlobFileName = bulkResultRequest.BlobFileName,
                    SourceFilePath = $"{bulkResultRequest.AoUkprn}/{BulkProcessStatus.Processing}",
                    FileStream = fileStream,
                    UserName = bulkResultRequest.PerformedBy
                });
            }

            var bulkResultResponse = await _internalApiClient.ProcessBulkResultsAsync(bulkResultRequest);
            return _mapper.Map<UploadResultsResponseViewModel>(bulkResultResponse);
        }
        
        public async Task<Stream> GetResultValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference)
        {
            var documentInfo = await _internalApiClient.GetDocumentUploadHistoryDetailsAsync(aoUkprn, blobUniqueReference);

            if (documentInfo != null && documentInfo.Status == (int)DocumentUploadStatus.Failed)
            {
                var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
                {
                    ContainerName = DocumentType.Results.ToString(),
                    BlobFileName = documentInfo.BlobFileName,
                    SourceFilePath = $"{aoUkprn}/{BulkProcessStatus.ValidationErrors}"
                });

                if (fileStream == null)
                {
                    var blobReadError = $"No FileStream found to download result validation errors. Method: DownloadFileAsync(ContainerName: {DocumentType.Results}, BlobFileName = {documentInfo.BlobFileName}, SourceFilePath = {aoUkprn}/{BulkProcessStatus.ValidationErrors})";
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

        public async Task<UlnResultsNotFoundViewModel> FindUlnResultsAsync(long aoUkprn, long Uln)
        {
            var response = await _internalApiClient.FindUlnAsync(aoUkprn, Uln);
            return _mapper.Map<UlnResultsNotFoundViewModel>(response);
        }

        public async Task<ResultDetailsViewModel> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var response = await _internalApiClient.GetResultDetailsAsync(aoUkprn, profileId, status);
            return _mapper.Map<ResultDetailsViewModel>(response);
        }

        public async Task<AddResultResponse> AddCoreResultAsync(long aoUkprn, ManageCoreResultViewModel viewModel)
        {
            var grades = await _internalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade);

            var selectedGrade = grades?.FirstOrDefault(x => x.Code.Equals(viewModel.SelectedGradeCode, StringComparison.InvariantCultureIgnoreCase));

            if (selectedGrade == null) return null;

            viewModel.LookupId = selectedGrade.Id;
            var request = _mapper.Map<AddResultRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.AddResultAsync(request);
        }

        public async Task<ManageCoreResultViewModel> GetAddCoreResultViewModelAsync(long aoUkprn, int profileId, int assessmentId, bool isChangeMode = false)
        {
            var response = await _internalApiClient.GetResultDetailsAsync(aoUkprn, profileId, RegistrationPathwayStatus.Active);
            
            if (response == null || response.PathwayAssessmentId != assessmentId || 
                (!isChangeMode && response.PathwayResultId.HasValue) || (isChangeMode && !response.PathwayResultId.HasValue))
                return null;

            var grades = await _internalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
            if (grades == null || !grades.Any())
                return null;

            grades.Insert(0, new LookupData { Code = string.Empty, Value = Content.Result.AddCoreResult.Option_Not_Received });
            return _mapper.Map<ManageCoreResultViewModel>(response, opt => opt.Items["grades"] = grades);
        }
    }
}