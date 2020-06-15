using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class RegistrationLoader : IRegistrationLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;

        public RegistrationLoader(IResultsAndCertificationInternalApiClient internalApiClient, IBlobStorageService blobStorageService, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
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
    }
}
