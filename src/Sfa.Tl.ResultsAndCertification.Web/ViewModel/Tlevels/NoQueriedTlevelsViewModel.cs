using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels
{
    public class NoQueriedTlevelsViewModel
    {
        public BreadcrumbModel BreadCrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Tlevels_Dashboard, RouteName = RouteConstants.TlevelsDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Tlevels_None_Queried }
                    }
                };
            }
        }
    }
}
