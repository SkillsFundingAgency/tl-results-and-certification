using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class TLevelConfirmedDetailsViewModel : TlevelSummary
    {
        public TLevelConfirmedDetailsViewModel()
        {
            Specialisms = new List<string>();
        }

        public int PathwayId { get; set; }
        public bool IsValid { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.ConfirmedTlevels
                };
            }
        }

        public BreadcrumbModel BreadCrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Tlevel_ViewAll, RouteName = RouteConstants.YourTlevels },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Tlevel_Details }
                    }
                };
            }
        }
    }
}
