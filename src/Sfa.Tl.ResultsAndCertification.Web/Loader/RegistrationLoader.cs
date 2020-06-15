using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class RegistrationLoader : IRegistrationLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrationLoader> _logger;

        public RegistrationLoader(IMapper mapper, ILogger<RegistrationLoader> logger, IResultsAndCertificationInternalApiClient internalApiClient, IBlobStorageService blobStorageService)
        {
            _mapper = mapper;
            _logger = logger;
            _internalApiClient = internalApiClient;
            _blobStorageService = blobStorageService;
        }

        public async Task<UploadRegistrationsResponseViewModel> ProcessBulkRegistrationsAsync(UploadRegistrationsRequestViewModel viewModel)
        {            
            var bulkRegistrationRequest = _mapper.Map<BulkRegistrationRequest>(viewModel);

            using (var fileStream = viewModel.File.OpenReadStream())
            {
                await _blobStorageService.UploadFileAsync(new BlobStorageData
                {
                    ContainerName = viewModel.DocumentType.ToString(),
                    BlobFileName = viewModel.BlobFileName,
                    SourceFilePath = $"{viewModel.AoUkprn}/{BulkRegistrationProcessStatus.Processing}",
                    FileStream = fileStream,
                    UserName = bulkRegistrationRequest.PerformedBy
                });
            }

            var bulkRegistrationResponse = await _internalApiClient.ProcessBulkRegistrationsAsync(bulkRegistrationRequest);
            return _mapper.Map<UploadRegistrationsResponseViewModel>(bulkRegistrationResponse);
        }

        public async Task<Stream> GetRegistrationValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference)
        {
            var tlevelDetails = await _internalApiClient.GetDocumentUploadHistoryDetailsAsync(aoUkprn, blobUniqueReference);

            if (tlevelDetails != null && tlevelDetails.Status == (int)DocumentUploadStatus.Failed)
            {
                var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
                {
                    ContainerName = DocumentType.Registrations.ToString(),
                    BlobFileName = tlevelDetails.BlobFileName,
                    SourceFilePath = $"{aoUkprn}/{BulkRegistrationProcessStatus.ValidationErrors}"
                });

                if(fileStream == null)
                {
                    var blobReadError = $"No FileStream found to download registration validation errors. Method: DownloadFileAsync(ContainerName: {DocumentType.Registrations}, BlobFileName = {tlevelDetails.BlobFileName}, SourceFilePath = {aoUkprn}/{BulkRegistrationProcessStatus.ValidationErrors})";
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
    }
}
