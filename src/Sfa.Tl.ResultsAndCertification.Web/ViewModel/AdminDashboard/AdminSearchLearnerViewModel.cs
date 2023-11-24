using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerViewModel
    {
        private const int LearnerResultsPageSize = 10;

        public AdminSearchLearnerViewModel() { }

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

        public string ErrorMessage { get; set; }

        public bool SearchKeyOrFiltersApplied
            => SearchLearnerCriteria?.IsSearchKeyApplied == true || SearchLearnerCriteria?.SearchLearnerFilters?.IsApplyFiltersSelected == true;

        public bool OnlySearchKeyApplied
            => SearchLearnerCriteria?.IsSearchKeyApplied == true || SearchLearnerCriteria?.SearchLearnerFilters?.IsApplyFiltersSelected != true;

        public bool IsSearchKeyUln
            => SearchLearnerCriteria?.IsSearchKeyApplied == true && SearchLearnerCriteria?.SearchKey?.IsLong() == true;

        public bool IsSearchKeyValid { get; set; }

        public bool ContainsLearnerResults
            => SearchLearnerDetailsList?.LearnerDetails != null && SearchLearnerDetailsList.LearnerDetails.Any();

        public bool ContainsMultipleLearnerResultsPages
            => ContainsLearnerResults && SearchLearnerDetailsList?.PagerInfo?.TotalItems > LearnerResultsPageSize;

        public void ClearSearchKey()
        {
            if (SearchLearnerCriteria == null)
            {
                return;
            }

            SearchLearnerCriteria.IsSearchKeyApplied = false;
            SearchLearnerCriteria.SearchKey = string.Empty;
            IsSearchKeyValid = false;
            ErrorMessage = string.Empty;
        }

        public void SetSearchKey(string searchKey)
        {
            SearchLearnerCriteria ??= new AdminSearchLearnerCriteriaViewModel();

            SearchLearnerCriteria.IsSearchKeyApplied = true;
            SearchLearnerCriteria.SearchKey = searchKey;

            if (string.IsNullOrWhiteSpace(searchKey) || (searchKey.IsLong() && !Regex.IsMatch(searchKey, Constants.UlnValidationRegex)))
            {
                IsSearchKeyValid = false;
                ErrorMessage = AdminSearchLearners.Validation_Enter_Valid_ULN_Or_Learners_Last_Name;
            }
            else
            {
                IsSearchKeyValid = true;
                ErrorMessage = string.Empty;
            }
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
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_Learner_Records }
            }
        };
    }
}