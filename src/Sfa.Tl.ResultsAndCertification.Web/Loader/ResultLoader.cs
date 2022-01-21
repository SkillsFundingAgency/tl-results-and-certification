using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.Collections.Generic;
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

        public async Task<ResultWithdrawnViewModel> GetResultWithdrawnViewModelAsync(long aoUkprn, int profileId)
        {
            var response = await _internalApiClient.GetResultDetailsAsync(aoUkprn, profileId, RegistrationPathwayStatus.Withdrawn);
            return _mapper.Map<ResultWithdrawnViewModel>(response);
        }

        public ResultNoAssessmentEntryViewModel GetResultNoAssessmentEntryViewModel(ResultDetailsViewModel resultDetails)
        {
            return _mapper.Map<ResultNoAssessmentEntryViewModel>(resultDetails);
        }

        public async Task<ResultDetailsViewModel> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var response = await _internalApiClient.GetResultDetailsAsync(aoUkprn, profileId, status);
            return _mapper.Map<ResultDetailsViewModel>(response);
        }

        public async Task<ResultDetailsViewModelNew> GetResultDetailsNewAsync(long aoUkprn, int profileId)
        {
            var response = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId);
            if (response.Pathway.Status != RegistrationPathwayStatus.Active)
                return null;

            return _mapper.Map<ResultDetailsViewModelNew>(response);
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

        public async Task<ManageCoreResultViewModel> GetManageCoreResultAsync(long aoUkprn, int profileId, int assessmentId, bool isChangeMode)
        {
            var response = await _internalApiClient.GetLearnerRecordAsync(aoUkprn, profileId, RegistrationPathwayStatus.Active);

            if (response == null)
                return null;

            var assessment = response.Pathway.PathwayAssessments.FirstOrDefault(p => p.Id == assessmentId);
            var hasResult = assessment?.Results?.Any() ?? false;

            if (assessment == null || (!isChangeMode && hasResult) || (isChangeMode && !hasResult))
                return null;

            var grades = await _internalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
            if (grades == null || !grades.Any())
                return null;

            if(isChangeMode)
                grades.Insert(grades.Count, new LookupData { Code = string.Empty, Value = Content.Result.ManageCoreResult.Option_Not_Received });

            return _mapper.Map<ManageCoreResultViewModel>(response, opt => 
            { 
                opt.Items["grades"] = grades;
                opt.Items["assessment"] = assessment; 
            });
        }

        public async Task<bool?> IsCoreResultChangedAsync(long aoUkprn, ManageCoreResultViewModel viewModel)
        {
            var existingResult = await _internalApiClient.GetResultDetailsAsync(aoUkprn, viewModel.ProfileId, RegistrationPathwayStatus.Active);

            if (existingResult == null || existingResult.PathwayResultId != viewModel.ResultId)
                return null;

            var isResultChanged = !existingResult.PathwayResultCode.Equals(viewModel.SelectedGradeCode, StringComparison.InvariantCultureIgnoreCase);
            return isResultChanged;
        }

        public async Task<ChangeResultResponse> ChangeCoreResultAsync(long aoUkprn, ManageCoreResultViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.SelectedGradeCode))
            {
                var grades = await _internalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade);

                var selectedGrade = grades?.FirstOrDefault(x => x.Code.Equals(viewModel.SelectedGradeCode, StringComparison.InvariantCultureIgnoreCase));

                if (selectedGrade == null) return null;

                viewModel.LookupId = selectedGrade.Id;
            }
            var request = _mapper.Map<ChangeResultRequest>(viewModel, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.ChangeResultAsync(request);
        }

        public async Task<IList<DataExportResponse>> GenerateResultsExportAsync(long aoUkprn, string requestedBy)
        {
            return await _internalApiClient.GenerateDataExportAsync(aoUkprn, DataExportType.Results, requestedBy);
        }

        public async Task<Stream> GetResultsDataFileAsync(long aoUkprn, Guid blobUniqueReference, ComponentType componentType)
        {
            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.DataExports.ToString(),
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                SourceFilePath = $"{aoUkprn}/{DataExportType.Results}/{componentType}"
            });

            if (fileStream == null)
            {
                var blobReadError = $"No FileStream found to download results data. Method: GetResultsDataFileAsync(ContainerName: {DocumentType.Assessments}, BlobFileName = {blobUniqueReference}, SourceFilePath = {aoUkprn}/{DataExportType.Assessments})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }
            return fileStream;
        }
    }
}