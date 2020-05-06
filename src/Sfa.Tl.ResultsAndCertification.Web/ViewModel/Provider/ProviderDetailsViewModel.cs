using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider
{
    public class YourProvidersViewModel
    {
        public IList<ProviderDetailsViewModel> Providers { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Dashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Provider_Your_Providers }
                    }
                };
            }
        }
    }

    public class ProviderDetailsViewModel
    {
        public int ProviderId { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }       
    }
}
