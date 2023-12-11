using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnerClearFiltersPost
{
    public class When_Called_And_Cache_Contains_Valid_Data : AdminDashboardControllerTestBase
    {
        private const string PageNumberKey = "pageNumber";
        private const int PageNumber = 1;

        private AdminSearchLearnerViewModel _viewModel;
        private IActionResult _result;

        public override void Given()
        {
            _viewModel = new AdminSearchLearnerViewModel
            {
                SearchLearnerCriteria = new AdminSearchLearnerCriteriaViewModel
                {
                    SearchKey = "johnson",
                    PageNumber = PageNumber,
                    SearchLearnerFilters = new AdminSearchLearnerFiltersViewModel
                    {
                        Search = "Barnsley College",
                        SelectedProviderId = 125,
                        AwardingOrganisations = new List<FilterLookupData>
                        {
                            CreateFilter(1, "NFCE", isSelected: true),
                            CreateFilter(2, "Pearson", isSelected: true),
                            CreateFilter(3, "City & Guilds", isSelected: true)
                        },
                        AcademicYears = new List<FilterLookupData>
                        {
                            CreateFilter(2020, "2020 to 2021", isSelected : true),
                            CreateFilter(2021, "2021 to 2022", isSelected : true),
                            CreateFilter(2022, "2022 to 2023", isSelected : true),
                            CreateFilter(2023, "2023 to 2024", isSelected : true)
                        }
                    }
                }
            };

            CacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey).Returns(_viewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnerClearFiltersAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminSearchLearnerViewModel>(
                p => p.SearchLearnerCriteria.SearchKey == _viewModel.SearchLearnerCriteria.SearchKey
                && p.SearchLearnerCriteria.PageNumber == PageNumber
                && p.SearchLearnerCriteria.SearchLearnerFilters.Search == string.Empty
                && !p.SearchLearnerCriteria.SearchLearnerFilters.SelectedProviderId.HasValue
                && p.SearchLearnerCriteria.SearchLearnerFilters.AwardingOrganisations.All(p => !p.IsSelected)
                && p.SearchLearnerCriteria.SearchLearnerFilters.AcademicYears.All(p => !p.IsSelected)));
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            AdminSearchLearnerCriteriaViewModel criteria = _viewModel.SearchLearnerCriteria;
            AdminSearchLearnerFiltersViewModel filters = criteria.SearchLearnerFilters;

            criteria.Should().NotBeNull();
            criteria.SearchKey.Should().Be(_viewModel.SearchLearnerCriteria.SearchKey);
            criteria.PageNumber.Should().Be(PageNumber);

            filters.Search.Should().BeEmpty();
            filters.SelectedProviderId.Should().NotHaveValue();
            filters.AwardingOrganisations.Should().AllSatisfy(p => p.IsSelected.Should().BeFalse());
            filters.AcademicYears.Should().AllSatisfy(p => p.IsSelected.Should().BeFalse());
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchLearnersRecords()
        {
            _result.Should().BeOfType<RedirectToRouteResult>();

            var result = _result as RedirectToRouteResult;
            result.RouteName.Should().Be(RouteConstants.AdminSearchLearnersRecords);
            result.RouteValues.Should().ContainKey(PageNumberKey);
            result.RouteValues[PageNumberKey].Should().Be(_viewModel.SearchLearnerCriteria.PageNumber);
        }
    }
}