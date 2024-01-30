using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnersGet
{
    public class When_Cache_Not_Empty_Only_Search_Key_Applied : AdminSearchLearnerTestBase
    {
        private AdminSearchLearnerViewModel _searchLearnerViewModel;
        private AdminSearchLearnerCriteriaViewModel _criteriaViewModel;
        private AdminSearchLearnerFiltersViewModel _filtersViewModel;
        private AdminSearchLearnerDetailsListViewModel _loadedSearchLearnerDetailsList;
        private AdminSearchLearnerDetailsViewModel _loadedLearnerDetails;
        private IActionResult _result;

        public override void Given()
        {
            _filtersViewModel = new AdminSearchLearnerFiltersViewModel
            {
                Search = string.Empty,
                SelectedProviderId = null,
                AwardingOrganisations = new List<FilterLookupData>
                {
                    CreateFilter(1, "NFCE"),
                    CreateFilter(2, "Pearson"),
                    CreateFilter(3, "City & Guilds")
                },
                AcademicYears = new List<FilterLookupData>
                {
                    CreateFilter(2020, "2020 to 2021"),
                    CreateFilter(2021, "2021 to 2022"),
                    CreateFilter(2022, "2022 to 2023"),
                    CreateFilter(2023, "2023 to 2024")
                }
            };

            _criteriaViewModel = new AdminSearchLearnerCriteriaViewModel
            {
                SearchKey = "smith",
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

            _loadedLearnerDetails = new AdminSearchLearnerDetailsViewModel
            {
                RegistrationPathwayId = 2000,
                Uln = 1234567890,
                LearnerName = "Kevin Smith",
                AwardingOrganisation = "Pearson",
                Provider = "Exeter College (10002370)",
                StartYear = "2022 to 2023"
            };

            _loadedSearchLearnerDetailsList = new AdminSearchLearnerDetailsListViewModel
            {
                LearnerDetails = new List<AdminSearchLearnerDetailsViewModel> { _loadedLearnerDetails },
                TotalRecords = 1,
                PagerInfo = new PagerViewModel
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    RecordFrom = 1,
                    RecordTo = 1,
                    StartPage = 1,
                    TotalItems = 1,
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
            var model = _result.ShouldBeViewResult<AdminSearchLearnerViewModel>();

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