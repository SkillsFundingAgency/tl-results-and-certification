using Microsoft.IdentityModel.Tokens;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Collections.Generic;
using System.Linq;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using SearchRegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.SearchRegistration.SearchRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration
{
    public class SearchRegistrationViewModel
    {
        private const int FirstPage = 1;
        private const int LearnerResultsPageSize = 10;

        public SearchRegistrationType SearchType { get; set; }

        public SearchRegistrationViewModel() { }

        public SearchRegistrationViewModel(SearchRegistrationType type, SearchRegistrationFiltersViewModel filters)
        {
            SearchType = type;
            Criteria.Filters = filters;
        }

        public SearchRegistrationCriteriaViewModel Criteria { get; set; } = new SearchRegistrationCriteriaViewModel();

        public SearchRegistrationDetailsListViewModel DetailsList { get; set; } = new SearchRegistrationDetailsListViewModel();

        public SearchRegistrationState State { get; set; } = SearchRegistrationState.NoSearch;

        public bool OnlySearchKeyApplied
            => Criteria?.IsSearchKeyApplied == true || Criteria?.Filters?.IsApplyFiltersSelected != true;

        public bool ContainsResults
            => DetailsList?.RegistrationDetails != null && DetailsList.RegistrationDetails.Any();

        public bool ContainsMultipleResultsPages
            => ContainsResults && DetailsList?.PagerInfo?.TotalItems > LearnerResultsPageSize;

        public void SetSearchKey(string searchKey)
        {
            Criteria ??= new SearchRegistrationCriteriaViewModel();
            Criteria.SearchKey = searchKey;
            Criteria.PageNumber = FirstPage;
        }

        public void ClearSearchKey()
        {
            Criteria ??= new SearchRegistrationCriteriaViewModel();
            Criteria.SearchKey = string.Empty;
            Criteria.PageNumber = FirstPage;
        }

        public void SetFilters(SearchRegistrationFiltersViewModel filtersViewModel)
        {
            Criteria ??= new SearchRegistrationCriteriaViewModel();
            Criteria.Filters = filtersViewModel;
            Criteria.PageNumber = FirstPage;
        }

        public void ClearFilters()
        {
            Criteria ??= new SearchRegistrationCriteriaViewModel();
            Criteria.Filters ??= new SearchRegistrationFiltersViewModel();

            SearchRegistrationFiltersViewModel filters = Criteria.Filters;

            filters.Search = string.Empty;
            filters.SelectedProviderId = null;

            if (!filters.AcademicYears.IsNullOrEmpty())
            {
                foreach (var filter in filters.AcademicYears)
                {
                    filter.IsSelected = false;
                }
            }

            Criteria.PageNumber = FirstPage;
        }

        public void SetRegistrationDetailsList(SearchRegistrationDetailsListViewModel searchRegistrationDetailsList)
        {
            DetailsList = searchRegistrationDetailsList;
            State = ContainsResults ? SearchRegistrationState.ResultsFound : SearchRegistrationState.ResultsNotFound;
        }

        public void ClearRegistrationDetailsList()
        {
            DetailsList?.RegistrationDetails?.Clear();
            State = SearchRegistrationState.NoSearch;
        }

        public PaginationModel Pagination => new()
        {
            PagerInfo = DetailsList?.PagerInfo,
            RouteName = RouteConstants.SearchRegistration,
            RouteAttributes = new Dictionary<string, string>
            {
                ["type"] = SearchType.ToString()
            },
            PaginationSummary = SearchRegistrationContent.PaginationSummary_Text
        };

        public string PageTitle
            => SearchType switch
            {
                SearchRegistrationType.Assessment => SearchRegistrationContent.Page_Title_Search_Assessment,
                SearchRegistrationType.Result => SearchRegistrationContent.Page_Title_Search_Result,
                SearchRegistrationType.PostResult => SearchRegistrationContent.Page_Title_Search_Romm_Appeal,
                _ => SearchRegistrationContent.Page_Title_Search_Registration
            };

        public string PageHeading
           => SearchType switch
           {
               SearchRegistrationType.Assessment => SearchRegistrationContent.Heading_Search_Assessment,
               SearchRegistrationType.Result => SearchRegistrationContent.Heading_Search_Result,
               SearchRegistrationType.PostResult => SearchRegistrationContent.Heading_Search_Romm_Appeal,
               _ => SearchRegistrationContent.Page_Title_Search_Registration
           };

        public BreadcrumbModel Breadcrumb => SearchType switch
        {
            SearchRegistrationType.Assessment => new()
            {
                BreadcrumbItems = new List<BreadcrumbItem>
                {
                    new() { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                    new() { DisplayName = BreadcrumbContent.Assessment_Dashboard, RouteName = RouteConstants.AssessmentDashboard },
                    new() { DisplayName = BreadcrumbContent.Search_For_Assessment_Entry }
                }
            },
            SearchRegistrationType.Result => new()
            {
                BreadcrumbItems = new List<BreadcrumbItem>
                {
                    new() { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                    new() { DisplayName = BreadcrumbContent.Result_Dashboard, RouteName = RouteConstants.ResultsDashboard },
                    new() { DisplayName = BreadcrumbContent.Search_For_Result_Entry }
                }
            },
            SearchRegistrationType.PostResult => new()
            {
                BreadcrumbItems = new List<BreadcrumbItem>
                {
                    new() { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                    new() { DisplayName = BreadcrumbContent.StartPostResultsService, RouteName = RouteConstants.StartReviewsAndAppeals },
                    new() { DisplayName = BreadcrumbContent.Search_For_Romm_Or_An_Appeal_Entry }
                }
            },
            _ => new()
            {
                BreadcrumbItems = new List<BreadcrumbItem>
                {
                    new() { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                    new() { DisplayName = BreadcrumbContent.Registration_Dashboard, RouteName = RouteConstants.RegistrationDashboard },
                    new() { DisplayName = BreadcrumbContent.Search_For_Registration_Entry }
                }
            }
        };
    }
}