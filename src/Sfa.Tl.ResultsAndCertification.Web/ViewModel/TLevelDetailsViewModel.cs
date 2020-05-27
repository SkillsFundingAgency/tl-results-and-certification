using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class TLevelDetailsViewModel
    {
        public TLevelDetailsViewModel()
        {
            Specialisms = new List<string>();
        }

        public int PathwayId { get; set; }
        public string PageTitle { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
        public bool ShowSomethingIsNotRight { get; set; }
        public bool ShowQueriedInfo { get; set; }
        public IEnumerable<string> Specialisms { get; set; }
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
