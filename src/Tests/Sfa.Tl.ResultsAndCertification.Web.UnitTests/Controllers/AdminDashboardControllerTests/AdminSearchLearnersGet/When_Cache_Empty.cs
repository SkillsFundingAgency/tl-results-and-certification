using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnersGet
{
    public class When_Cache_Empty : AdminDashboardControllerTestBase
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
            filters.Search.Should().BeEmpty();
            filters.SelectedProviderId.Should().NotHaveValue();
            filters.IsApplyFiltersSelected.Should().BeFalse();

            filters.AwardingOrganisations.Should().HaveCount(3);
            filters.AwardingOrganisations[0].Id.Should().Be(1);
            filters.AwardingOrganisations[0].Name.Should().Be("NFCE");
            filters.AwardingOrganisations[0].IsSelected.Should().BeFalse();
            filters.AwardingOrganisations[1].Id.Should().Be(2);
            filters.AwardingOrganisations[1].Name.Should().Be("Pearson");
            filters.AwardingOrganisations[1].IsSelected.Should().BeFalse();
            filters.AwardingOrganisations[2].Id.Should().Be(3);
            filters.AwardingOrganisations[2].Name.Should().Be("City & Guilds");
            filters.AwardingOrganisations[2].IsSelected.Should().BeFalse();

            filters.AcademicYears.Should().HaveCount(4);
            filters.AcademicYears[0].Id.Should().Be(2020);
            filters.AcademicYears[0].Name.Should().Be("2020 to 2021");
            filters.AcademicYears[0].IsSelected.Should().BeFalse();
            filters.AcademicYears[1].Id.Should().Be(2021);
            filters.AcademicYears[1].Name.Should().Be("2021 to 2022");
            filters.AcademicYears[1].IsSelected.Should().BeFalse();
            filters.AcademicYears[2].Id.Should().Be(2022);
            filters.AcademicYears[2].Name.Should().Be("2022 to 2023");
            filters.AcademicYears[2].IsSelected.Should().BeFalse();
            filters.AcademicYears[3].Id.Should().Be(2023);
            filters.AcademicYears[3].Name.Should().Be("2023 to 2024");
            filters.AcademicYears[3].IsSelected.Should().BeFalse();

            model.Pagination.PagerInfo.Should().BeNull();
            model.Pagination.RouteName.Should().Be(RouteConstants.AdminSearchLearnersRecords);
            model.Pagination.PaginationSummary.Should().Be(AdminSearchLearners.PaginationSummary_Text);

            model.Breadcrumb.BreadcrumbItems.Should().HaveCount(2);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.AdminHome);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Search_Learner_Records);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().BeNullOrEmpty();
        }
    }
}