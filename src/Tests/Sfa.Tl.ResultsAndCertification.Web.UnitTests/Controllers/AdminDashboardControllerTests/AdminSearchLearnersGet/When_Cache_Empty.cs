using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnersGet
{
    public class When_Cache_Empty : AdminSearchLearnerTestBase
    {
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

            CacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey).Returns(null as AdminSearchLearnerViewModel);
            AdminDashboardLoader.GetAdminSearchLearnerFiltersAsync().Returns(_filtersViewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnersAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).GetAdminSearchLearnerFiltersAsync();
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminSearchLearnerViewModel>(
                p => p.SearchLearnerCriteria.SearchKey == string.Empty
                && !p.SearchLearnerCriteria.PageNumber.HasValue
                && p.SearchLearnerCriteria.SearchLearnerFilters.Search == string.Empty
                && !p.SearchLearnerCriteria.SearchLearnerFilters.SelectedProviderId.HasValue
                && p.SearchLearnerCriteria.SearchLearnerFilters.AwardingOrganisations.All(p => !p.IsSelected)
                && p.SearchLearnerCriteria.SearchLearnerFilters.AcademicYears.All(p => !p.IsSelected)));
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
            criteria.SearchKey.Should().BeEmpty();
            criteria.PageNumber.Should().NotHaveValue();
            criteria.IsSearchKeyApplied.Should().BeFalse();
            criteria.AreFiltersApplied.Should().BeFalse();

            AdminSearchLearnerFiltersViewModel filters = criteria.SearchLearnerFilters;
            filters.Should().BeEquivalentTo(_filtersViewModel);

            PaginationModel pagination = model.Pagination;
            pagination.PagerInfo.Should().BeNull();

            AssertPaginationRouteNameAndSummary(pagination);
            AssertBreadcrumb(model.Breadcrumb);
        }
    }
}