using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminProviderLoader
    {
        Task<AdminProviderDetailsViewModel> GetProviderDetailsViewModel(int providerId);

        Task<AdminEditProviderViewModel> GetEditProviderViewModel(int providerId);

        Task<UpdateProviderResponse> SubmitUpdateProviderRequest(AdminEditProviderViewModel viewModel);
    }
}