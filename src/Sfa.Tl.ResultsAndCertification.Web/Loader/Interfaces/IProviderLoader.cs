using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IProviderLoader
    {
        Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn);
        Task<IEnumerable<ProviderLookupData>> GetProviderLookupDataAsync(string name, bool isExactMatch);
        Task<ProviderTlevelsViewModel> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId);
        Task<bool> AddProviderTlevelsAsync(ProviderTlevelsViewModel viewModel);
        Task<ProviderViewModel> GetViewProviderTlevelViewModelAsync(long aoUkprn, int providerId);
        Task<IList<ProviderDetailsViewModel>> GetTqAoProviderDetailsAsync(long aoUkprn);
        Task<ProviderTlevelDetailsViewModel> GetTqProviderTlevelDetailsAsync(long aoUkprn, int tqProviderId);
        Task<bool> RemoveTqProviderTlevelAsync(long aoUkprn, int tqProviderId);
        Task<YourProvidersViewModel> GetYourProvidersAsync(long aoUkprn);
        Task<bool> HasAnyTlevelSetupForProviderAsync(long aoUkprn, int tlProviderId);
    }
}
