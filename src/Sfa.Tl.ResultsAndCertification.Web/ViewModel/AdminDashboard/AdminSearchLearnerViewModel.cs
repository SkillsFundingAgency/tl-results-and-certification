using Microsoft.IdentityModel.Tokens;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Enum;
using System.Collections.Generic;
using System.Linq;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerViewModel
    {
        private const int FirstPage = 1;
        private const int LearnerResultsPageSize = 10;

        public AdminSearchLearnerViewModel() { }

        public AdminSearchLearnerViewModel(AdminSearchLearnerFiltersViewModel adminSearchLearnerFiltersViewModel)
        {
            SearchLearnerCriteria.SearchLearnerFilters = adminSearchLearnerFiltersViewModel;
        }

        public AdminSearchLearnerCriteriaViewModel SearchLearnerCriteria { get; set; } = new AdminSearchLearnerCriteriaViewModel();

        public AdminSearchLearnerDetailsListViewModel SearchLearnerDetailsList { get; set; } = new AdminSearchLearnerDetailsListViewModel();

        public AdminSearchState State { get; set; } = AdminSearchState.NoSearch;

        public bool ContainsLearnerResults
            => SearchLearnerDetailsList?.LearnerDetails != null && SearchLearnerDetailsList.LearnerDetails.Any();

        public bool ContainsMultipleLearnerResultsPages
            => ContainsLearnerResults && SearchLearnerDetailsList?.PagerInfo?.TotalItems > LearnerResultsPageSize;

        public void SetSearchKey(string searchKey)
        {
            SearchLearnerCriteria ??= new AdminSearchLearnerCriteriaViewModel();

            SearchLearnerCriteria.SearchKey = searchKey;
            SearchLearnerCriteria.PageNumber = FirstPage;
        }

        public void ClearSearchKey()
        {
            SearchLearnerCriteria ??= new AdminSearchLearnerCriteriaViewModel();

            SearchLearnerCriteria.SearchKey = string.Empty;
            SearchLearnerCriteria.PageNumber = FirstPage;
        }

        public void SetFilters(AdminSearchLearnerFiltersViewModel filtersViewModel)
        {
            SearchLearnerCriteria ??= new AdminSearchLearnerCriteriaViewModel();
            SearchLearnerCriteria.SearchLearnerFilters ??= new AdminSearchLearnerFiltersViewModel();

            SearchLearnerCriteria.SearchLearnerFilters = filtersViewModel;
            SearchLearnerCriteria.PageNumber = FirstPage;
        }

        public void ClearFilters()
        {
            SearchLearnerCriteria ??= new AdminSearchLearnerCriteriaViewModel();
            SearchLearnerCriteria.SearchLearnerFilters ??= new AdminSearchLearnerFiltersViewModel();

            AdminSearchLearnerFiltersViewModel filters = SearchLearnerCriteria.SearchLearnerFilters;

            filters.Provider = string.Empty;

            if (!filters.AwardingOrganisations.IsNullOrEmpty())
            {
                foreach (var filter in filters.AwardingOrganisations)
                {
                    filter.IsSelected = false;
                }
            }

            if (!filters.AcademicYears.IsNullOrEmpty())
            {
                foreach (var filter in filters.AcademicYears)
                {
                    filter.IsSelected = false;
                }
            }

            SearchLearnerCriteria.PageNumber = FirstPage;
        }

        public void SetLearnerDetails(AdminSearchLearnerDetailsListViewModel learnerDetailsListViewModel)
        {
            SearchLearnerDetailsList = learnerDetailsListViewModel;
            State = ContainsLearnerResults ? AdminSearchState.ResultsFound : AdminSearchState.ResultsNotFound;
        }

        public void ClearLearnerDetails()
        {
            SearchLearnerDetailsList?.LearnerDetails?.Clear();
            State = AdminSearchState.NoSearch;
        }

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
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.AdminHome },
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_Learner_Records }
            }
        };
    }
}