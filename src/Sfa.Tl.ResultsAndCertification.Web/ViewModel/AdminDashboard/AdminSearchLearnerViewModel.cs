using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Enum;
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

        public AdminSearchLearnerCriteriaViewModel SearchLearnerCriteria { get; set; } = new AdminSearchLearnerCriteriaViewModel();

        public AdminSearchLearnerDetailsListViewModel SearchLearnerDetailsList { get; set; } = new AdminSearchLearnerDetailsListViewModel();

        public AdminSearchState State { get; set; } = AdminSearchState.PageFirstDisplayed;

        public string LearnersNotFoundHeader { get; set; }

        public string LearnersNotFoundMessage { get; set; }

        public string ErrorMessage { get; set; }

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
            SearchLearnerCriteria ??= new AdminSearchLearnerCriteriaViewModel();

            State = AdminSearchState.PageFirstDisplayed;

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

        public void SetLearnerDetails(AdminSearchLearnerDetailsListViewModel learnerDetailsListViewModel)
        {
            SearchLearnerDetailsList = learnerDetailsListViewModel;

            if (ContainsLearnerResults)
            {
                State = AdminSearchState.ResultsFound;
                LearnersNotFoundHeader = string.Empty;
                LearnersNotFoundMessage = string.Empty;
            }
            else
            {
                State = AdminSearchState.ResultsNotFound;
                LearnersNotFoundHeader = string.Format(AdminSearchLearners.Heading_SearchKey_Not_Found, SearchLearnerCriteria?.SearchKey);
                LearnersNotFoundMessage = IsSearchKeyUln ? AdminSearchLearners.Para_We_Cannot_Find_Learner_ULN : AdminSearchLearners.Para_We_Cannot_Find_Learner_Surname;
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