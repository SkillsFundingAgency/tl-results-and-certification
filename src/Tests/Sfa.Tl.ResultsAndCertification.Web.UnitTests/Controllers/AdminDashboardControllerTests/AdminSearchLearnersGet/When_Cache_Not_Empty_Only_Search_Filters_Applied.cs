using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnersGet
{
    public class When_Cache_Not_Empty_Only_Search_Filters_Applied : AdminSearchLearnerTestBase
    {
        private AdminSearchLearnerViewModel _searchLearnerViewModel;
        private AdminSearchLearnerCriteriaViewModel _criteriaViewModel;
        private AdminSearchLearnerFiltersViewModel _filtersViewModel;

        private AdminSearchLearnerDetailsListViewModel _loadedSearchLearnerDetailsList;
        private AdminSearchLearnerDetailsViewModel _firstLoadedLearnerDetails, _secondLoadedLearnerDetails;
        private IActionResult _result;

        public override void Given()
        {
            _filtersViewModel = new AdminSearchLearnerFiltersViewModel
            {
                Search = "Exeter College",
                SelectedProviderId = 16,
                AwardingOrganisations = new List<FilterLookupData>
                {
                    CreateFilter(1, "NFCE", isSelected: true),
                    CreateFilter(2, "Pearson"),
                    CreateFilter(3, "City & Guilds")
                },
                AcademicYears = new List<FilterLookupData>
                {
                    CreateFilter(2020, "2020 to 2021", isSelected: true),
                    CreateFilter(2021, "2021 to 2022"),
                    CreateFilter(2022, "2022 to 2023"),
                    CreateFilter(2023, "2023 to 2024")
                }
            };

            _criteriaViewModel = new AdminSearchLearnerCriteriaViewModel
            {
                SearchKey = string.Empty,
                PageNumber = 1,
                SearchLearnerFilters = _filtersViewModel
            };

            _searchLearnerViewModel = new AdminSearchLearnerViewModel
            {
                SearchLearnerCriteria = _criteriaViewModel,
                SearchLearnerDetailsList = new AdminSearchLearnerDetailsListViewModel
                {
                    LearnerDetails = new List<AdminSearchLearnerDetailsViewModel>()
                }
            };

            CacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey).Returns(_searchLearnerViewModel);

            _firstLoadedLearnerDetails = new AdminSearchLearnerDetailsViewModel
            {
                RegistrationPathwayId = 2000,
                Uln = 1234567890,
                LearnerName = "Kevin Smith",
                AwardingOrganisation = "NFCE",
                Provider = "Exeter College (10002370)",
                StartYear = "2020 to 2021"
            };

            _secondLoadedLearnerDetails = new AdminSearchLearnerDetailsViewModel
            {
                RegistrationPathwayId = 3537,
                Uln = 9876543210,
                LearnerName = "Emily Taylor",
                AwardingOrganisation = "NFCE",
                Provider = "Exeter College (10002370)",
                StartYear = "2020 to 2021"
            };

            _loadedSearchLearnerDetailsList = new AdminSearchLearnerDetailsListViewModel
            {
                LearnerDetails = new List<AdminSearchLearnerDetailsViewModel> { _firstLoadedLearnerDetails, _secondLoadedLearnerDetails },
                TotalRecords = 1,
                PagerInfo = new PagerViewModel
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    RecordFrom = 1,
                    RecordTo = 2,
                    StartPage = 1,
                    TotalItems = 2,
                    TotalPages = 1
                }
            };

            AdminDashboardLoader.GetAdminSearchLearnerDetailsListAsync(_criteriaViewModel).Returns(_loadedSearchLearnerDetailsList);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnersAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).GetAdminSearchLearnerDetailsListAsync(_criteriaViewModel);
            CacheService.Received(1).SetAsync(CacheKey, _searchLearnerViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull().And.BeOfType<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Model.Should().NotBeNull().And.BeOfType<AdminSearchLearnerViewModel>();

            var model = viewResult.Model as AdminSearchLearnerViewModel;
            model.State.Should().Be(AdminSearchState.ResultsFound);
            model.ContainsLearnerResults.Should().BeTrue();
            model.ContainsMultipleLearnerResultsPages.Should().BeFalse();

            AdminSearchLearnerDetailsListViewModel detailsList = model.SearchLearnerDetailsList;
            detailsList.Should().BeEquivalentTo(_loadedSearchLearnerDetailsList);

            AdminSearchLearnerCriteriaViewModel criteria = model.SearchLearnerCriteria;
            criteria.Should().BeEquivalentTo(_criteriaViewModel);

            AdminSearchLearnerFiltersViewModel filters = criteria.SearchLearnerFilters;
            filters.Should().BeEquivalentTo(_filtersViewModel);

            PaginationModel pagination = model.Pagination;
            pagination.PagerInfo.Should().BeEquivalentTo(_loadedSearchLearnerDetailsList.PagerInfo);

            AssertPaginationRouteNameAndSummary(pagination);
            AssertBreadcrumb(model.Breadcrumb);
        }
    }
}