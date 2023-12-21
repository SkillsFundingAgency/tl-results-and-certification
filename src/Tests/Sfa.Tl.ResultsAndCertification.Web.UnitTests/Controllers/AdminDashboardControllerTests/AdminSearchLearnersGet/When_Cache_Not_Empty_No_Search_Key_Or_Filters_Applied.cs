using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnersGet
{
    public class When_Cache_Not_Empty_No_Search_Key_Or_Filters_Applied : AdminSearchLearnerTestBase
    {
        private AdminSearchLearnerCriteriaViewModel _criteriaViewModel;
        private AdminSearchLearnerFiltersViewModel _filtersViewModel;
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
                SearchKey = string.Empty,
                PageNumber = null,
                SearchLearnerFilters = _filtersViewModel
            };

            var searchLearnerViewModel = new AdminSearchLearnerViewModel
            {
                SearchLearnerCriteria = _criteriaViewModel,
                SearchLearnerDetailsList = new AdminSearchLearnerDetailsListViewModel
                {
                    LearnerDetails = new List<AdminSearchLearnerDetailsViewModel>
                    {
                        new AdminSearchLearnerDetailsViewModel
                        {
                            RegistrationPathwayId = 1500,
                            Uln = 1111111111,
                            LearnerName = "John Smith",
                            Provider = "Barnsley College (10000536)",
                            StartYear = "2020 to 2021",
                            AwardingOrganisation = "Pearson",
                        }
                    }
                }
            };

            CacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey).Returns(searchLearnerViewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnersAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            AdminDashboardLoader.DidNotReceive().GetAdminSearchLearnerFiltersAsync();
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<AdminSearchLearnerViewModel>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull().And.BeOfType<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Model.Should().NotBeNull().And.BeOfType<AdminSearchLearnerViewModel>();

            var model = viewResult.Model as AdminSearchLearnerViewModel;
            model.State.Should().Be(AdminSearchState.NoSearch);
            model.ContainsLearnerResults.Should().BeFalse();
            model.ContainsMultipleLearnerResultsPages.Should().BeFalse();

            AdminSearchLearnerDetailsListViewModel detailsList = model.SearchLearnerDetailsList;
            detailsList.Should().NotBeNull();
            detailsList.LearnerDetails.Should().BeEmpty();
            detailsList.TotalRecords.Should().Be(0);
            detailsList.PagerInfo.Should().BeNull();

            AdminSearchLearnerCriteriaViewModel criteria = model.SearchLearnerCriteria;
            criteria.Should().BeEquivalentTo(_criteriaViewModel);

            AdminSearchLearnerFiltersViewModel filters = criteria.SearchLearnerFilters;
            filters.Should().BeEquivalentTo(_filtersViewModel);

            PaginationModel pagination = model.Pagination;
            pagination.PagerInfo.Should().BeNull();

            AssertPaginationRouteNameAndSummary(pagination);
            AssertBreadcrumb(model.Breadcrumb);
        }
    }
}