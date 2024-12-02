using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminProviderLoader : IAdminProviderLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _apiClient;
        private readonly IMapper _mapper;

        public AdminProviderLoader(IResultsAndCertificationInternalApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task<AdminProviderDetailsViewModel> GetProviderDetailsViewModel(int providerId)
        {
            GetProviderResponse response = await _apiClient.GetProviderAsync(providerId);
            return _mapper.Map<AdminProviderDetailsViewModel>(response);
        }

        public async Task<AdminEditProviderViewModel> GetEditProviderViewModel(int providerId)
        {
            GetProviderResponse response = await _apiClient.GetProviderAsync(providerId);
            return _mapper.Map<AdminEditProviderViewModel>(response);
        }

        public Task<UpdateProviderResponse> SubmitUpdateProviderRequest(AdminEditProviderViewModel viewModel)
        {
            UpdateProviderRequest request = _mapper.Map<UpdateProviderRequest>(viewModel);
            return _apiClient.UpdateProviderAsync(request);
        }

        public Task<AddProviderResponse> SubmitAddProviderRequest(AdminAddProviderViewModel viewModel)
        {
            AddProviderRequest request = _mapper.Map<AddProviderRequest>(viewModel);
            return _apiClient.AddProviderAsync(request);
        }
    }
}