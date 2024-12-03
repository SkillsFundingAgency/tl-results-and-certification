using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminBannerLoader
    {
        Task<AdminFindBannerViewModel> SearchBannersAsync(AdminFindBannerCriteriaViewModel criteria = null);

        Task<AdminBannerDetailsViewModel> GetBannerDetailsViewModel(int bannerId);

        Task<AdminEditBannerViewModel> GetEditBannerViewModel(int bannerId);

        Task<bool> SubmitUpdateBannerRequest(AdminEditBannerViewModel viewModel);

        Task<AddBannerResponse> SubmitAddBannerRequest(AdminAddBannerViewModel viewModel);
    }
}