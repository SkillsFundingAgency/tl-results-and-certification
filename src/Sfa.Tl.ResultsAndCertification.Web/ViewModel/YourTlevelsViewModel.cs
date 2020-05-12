using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{

    public class YourTlevelsViewModel
    {
        public YourTlevelsViewModel()
        {
            ConfirmedTlevels = new List<YourTlevelViewModel>();
            QueriedTlevels = new List<YourTlevelViewModel>();
        }

        public bool IsAnyReviewPending { get; set; }
        public List<YourTlevelViewModel> ConfirmedTlevels { get; set; }
        public List<YourTlevelViewModel> QueriedTlevels { get; set; }

        public BreadcrumbModel BreadCrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Dashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Tlevel_ViewAll }
                    }
                };
            }
        }
    }
}
