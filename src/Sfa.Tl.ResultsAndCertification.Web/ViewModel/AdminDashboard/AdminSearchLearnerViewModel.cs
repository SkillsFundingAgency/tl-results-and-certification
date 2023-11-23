using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerViewModel
    {
        public AdminSearchLearnerViewModel(AdminSearchLearnerFiltersViewModel searchLearnerFilters)
        {
            SearchLearnerCriteria = new AdminSearchLearnerCriteriaViewModel
            {
                SearchLearnerFilters = searchLearnerFilters
            };
        }

        public AdminSearchLearnerCriteriaViewModel SearchLearnerCriteria { get; set; }

        public AdminSearchLearnerDetailsListViewModel SearchLearnerDetailsList { get; set; }

        public BreadcrumbModel Breadcrumb => new()
        {
            BreadcrumbItems = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_Learner_Records }
            }
        };

        public PaginationModel Pagination
        {
            get
            {
                return new PaginationModel
                {
                    PagerInfo = SearchLearnerDetailsList?.PagerInfo,
                    RouteName = RouteConstants.AdminSearchLearnersRecords,
                    PaginationSummary = AdminSearchLearners.PaginationSummary_Text
                };
            }
        }
    }
}