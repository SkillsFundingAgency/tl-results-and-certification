using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class RegistrationLoader : IRegistrationLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public RegistrationLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<UploadRegistrationsResponseViewModel> ProcessBulkRegistrationsAsync(UploadRegistrationsRequestViewModel viewModel)
        {
            var bulkRegistrationResponse = await _internalApiClient.ProcessBulkRegistrationsAsync(new BulkRegistrationRequest());
            return _mapper.Map<UploadRegistrationsResponseViewModel>(bulkRegistrationResponse);
        }        
    }
}
