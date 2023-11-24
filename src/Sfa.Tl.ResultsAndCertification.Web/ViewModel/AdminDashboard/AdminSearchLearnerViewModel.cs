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
        public AdminSearchLearnerViewModel(AdminSearchLearnerFiltersViewModel adminSearchLearnerFiltersViewModel)
        {
            SearchLearnerCriteria.SearchLearnerFilters = adminSearchLearnerFiltersViewModel;
        }

        public AdminSearchLearnerViewModel(AdminSearchLearnerCriteriaViewModel searchLearnerCriteria, AdminSearchLearnerDetailsListViewModel searchLearnerDetailsList)
        {
            SearchLearnerCriteria = searchLearnerCriteria;
            SearchLearnerDetailsList = searchLearnerDetailsList;
        }

        public AdminSearchLearnerCriteriaViewModel SearchLearnerCriteria { get; set; } = new AdminSearchLearnerCriteriaViewModel();

        public AdminSearchLearnerDetailsListViewModel SearchLearnerDetailsList { get; set; } = new AdminSearchLearnerDetailsListViewModel();

        public bool SearchKeyOrFiltersApplied
            => SearchLearnerCriteria?.IsSearchKeyApplied == true || SearchLearnerCriteria?.SearchLearnerFilters?.IsApplyFiltersSelected == true;

        public PaginationModel Pagination => new()
        {
            PagerInfo = SearchLearnerDetailsList?.PagerInfo,
            RouteName = RouteConstants.AdminSearchLearnersRecords,
            PaginationSummary = AdminSearchLearners.PaginationSummary_Text
        };

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