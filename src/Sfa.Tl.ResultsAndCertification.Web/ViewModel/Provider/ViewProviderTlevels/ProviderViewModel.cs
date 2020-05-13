using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.Linq;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels
{
    public class ProviderViewModel
    {
        public ProviderViewModel()
        {
            Tlevels = new List<TlevelViewModel>();
        }

        public int ProviderId { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }
        public bool IsNavigatedFromFindProvider { get; set; }
        public bool IsNavigatedFromYourProvider { get; set; }
        public bool AnyTlevelsAvailable { get { return Tlevels.Any(x => x.TqProviderId.HasValue); } }
        public bool ShowAnotherTlevelButton { get { return Tlevels.Any(x => !x.TqProviderId.HasValue); } }
        public IEnumerable<TlevelViewModel> ProviderTlevels { get { return Tlevels.Where(x => x.TqProviderId.HasValue); } }
        public IList<TlevelViewModel> Tlevels { get; set; }
       
        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Provider_Your_Providers, RouteName = RouteConstants.YourProviders },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Provider_Tlevels }
                    }
                };
            }
        }
    }
}
