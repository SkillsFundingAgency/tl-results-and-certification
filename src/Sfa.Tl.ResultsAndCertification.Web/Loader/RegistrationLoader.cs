using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
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
                    ContainerName = bulkRegistrationRequest.DocumentType.ToString(),
                    BlobFileName = bulkRegistrationRequest.BlobFileName,
                    SourceFilePath = $"{bulkRegistrationRequest.AoUkprn}/{BulkRegistrationProcessStatus.Processing}",
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

        public async Task<SelectProviderViewModel> GetRegisteredTqAoProviderDetailsAsync(long aoUkprn)
        {
            var providerDetails = await _internalApiClient.GetTqAoProviderDetailsAsync(aoUkprn);
            return _mapper.Map<SelectProviderViewModel>(providerDetails);
        }

        public async Task<SelectCoreViewModel> GetRegisteredProviderPathwayDetailsAsync(long aoUkprn, long providerUkprn)
        {
            var providerPathways = await _internalApiClient.GetRegisteredProviderPathwayDetailsAsync(aoUkprn, providerUkprn);
            return _mapper.Map<SelectCoreViewModel>(providerPathways);
        }

        public async Task<PathwaySpecialismsViewModel> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId)
        {
            var pathwaySpecialisms = await _internalApiClient.GetPathwaySpecialismsByPathwayLarIdAsync(aoUkprn, pathwayLarId);
            return _mapper.Map<PathwaySpecialismsViewModel>(pathwaySpecialisms);
        }

        public async Task<UlnCannotBeRegisteredViewModel> IsUlnRegisteredAsync(long aoUkprn, string Uln)
        {
            var model = new UlnCannotBeRegisteredViewModel();
            
            if (1 == 1)
            {
                model.IsRegisteredWithOtherAo = true;
                model.RegistrationProfileId = 1;
                model.TechnicalSupportEmailAddress = "TODO@email.com";
            }

           return await Task.Run(() => model);
        }

        public async Task<bool> AddRegistrationAsync(long aoUkprn, RegistrationViewModel model)
        {
            var registrationModel = _mapper.Map<RegistrationRequest>(model, opt => opt.Items["aoUkprn"] = aoUkprn);
            return await _internalApiClient.AddRegistrationAsync(registrationModel);
        }
    }
}
