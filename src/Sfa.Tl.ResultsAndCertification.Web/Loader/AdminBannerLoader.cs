using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminBannerLoader : IAdminBannerLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _apiClient;
        private readonly IMapper _mapper;

        public AdminBannerLoader(IResultsAndCertificationInternalApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task<AdminFindBannerViewModel> SearchBannersAsync(AdminFindBannerCriteriaViewModel criteria = null)
        {
            AdminSearchBannerRequest searchRequest = _mapper.Map<AdminSearchBannerRequest>(criteria) ?? new AdminSearchBannerRequest();

            PagedResponse<SearchBannerDetail> banners = await _apiClient.SearchBannersAsync(searchRequest);
            AdminFindBannerViewModel result = _mapper.Map<AdminFindBannerViewModel>(banners);

            result.SearchCriteriaViewModel = criteria;
            return result;
        }

        public async Task<AdminBannerDetailsViewModel> GetBannerDetailsViewModel(int bannerId)
        {
            GetBannerResponse response = await _apiClient.GetBannerAsync(bannerId);
            return _mapper.Map<AdminBannerDetailsViewModel>(response);
        }

        public async Task<AdminEditBannerViewModel> GetEditBannerViewModel(int bannerId)
        {
            GetBannerResponse response = await _apiClient.GetBannerAsync(bannerId);
            return _mapper.Map<AdminEditBannerViewModel>(response);
        }

        public Task<bool> SubmitUpdateBannerRequest(AdminEditBannerViewModel viewModel)
        {
            UpdateBannerRequest request = _mapper.Map<UpdateBannerRequest>(viewModel);
            return _apiClient.UpdateBannerAsync(request);
        }

        public Task<AddBannerResponse> SubmitAddBannerRequest(AdminAddBannerViewModel viewModel)
        {
            AddBannerRequest request = _mapper.Map<AddBannerRequest>(viewModel);
            return _apiClient.AddBannerAsync(request);
        }
    }
}