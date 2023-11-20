using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerViewModel
    {
        public AdminSearchLearnerViewModel(AdminSearchLearnerFiltersViewModel filtersViewModel)
        {
            SearchLearnerFilters = filtersViewModel;
        }

        public AdminSearchLearnerFiltersViewModel SearchLearnerFilters { get; set; }

        public string SearchKey { get; set; }

        public int? PageNumber { get; set; }

        public bool IsSearchKeyApplied { get; set; }

        public BreadcrumbModel Breadcrumb => new()
        {
            BreadcrumbItems = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_Learner_Records }
            }
        };
    }
}