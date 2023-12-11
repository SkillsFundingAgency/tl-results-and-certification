using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnerRecordsApplyFiltersPost
{
    public class When_Called_And_Cache_Contains_Valid_Data_Multiple_Existing_Provider_Search : AdminDashboardControllerTestBase
    {
        private const string PageNumberKey = "pageNumber";
        private const int PageNumber = 1;

        private AdminSearchLearnerFiltersViewModel _filtersViewModel;
        private AdminSearchLearnerCriteriaViewModel _criteriaViewModel;
        private AdminSearchLearnerViewModel _adminSearchLearnerViewModel;
        private IActionResult _result;

        public override void Given()
        {
            _filtersViewModel = new AdminSearchLearnerFiltersViewModel
            {
                Search = "Multiple existing providers"
            };

            _criteriaViewModel = new AdminSearchLearnerCriteriaViewModel
            {
                SearchKey = "johnson",
                PageNumber = PageNumber
            };

            _adminSearchLearnerViewModel = new AdminSearchLearnerViewModel
            {
                SearchLearnerCriteria = _criteriaViewModel
            };

            CacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey).Returns(_adminSearchLearnerViewModel);

            var providers = new[]
            {
                new ProviderLookupData
                {
                    Id = 1,
                    DisplayName = "First provider"
                },
                new ProviderLookupData
                {
                    Id = 2,
                    DisplayName = "Second provider"
                }
            };

            ProviderLoader.GetProviderLookupDataAsync(_filtersViewModel.Search, isExactMatch: true).Returns(providers);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnerRecordsApplyFiltersAsync(_filtersViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ProviderLoader.Received(1).GetProviderLookupDataAsync(_filtersViewModel.Search, isExactMatch: true);
            CacheService.Received(1).GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminSearchLearnerViewModel>(
                p => p.SearchLearnerCriteria.SearchKey == _criteriaViewModel.SearchKey
                && p.SearchLearnerCriteria.PageNumber == _criteriaViewModel.PageNumber
                && p.SearchLearnerCriteria.SearchLearnerFilters.Search == _filtersViewModel.Search
                && p.SearchLearnerCriteria.SearchLearnerFilters.SelectedProviderId == 0));
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            AdminSearchLearnerCriteriaViewModel criteria = _adminSearchLearnerViewModel.SearchLearnerCriteria;
            AdminSearchLearnerFiltersViewModel filters = criteria.SearchLearnerFilters;

            criteria.Should().NotBeNull();
            criteria.SearchKey.Should().Be(_criteriaViewModel.SearchKey);
            criteria.PageNumber.Should().Be(_criteriaViewModel.PageNumber);

            filters.Search.Should().Be(_filtersViewModel.Search);
            filters.SelectedProviderId.Should().Be(0);
            filters.AcademicYears.Should().BeEquivalentTo(_filtersViewModel.AcademicYears);
            filters.AwardingOrganisations.Should().BeEquivalentTo(_filtersViewModel.AwardingOrganisations);
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchLearnersRecords()
        {
            _result.Should().BeOfType<RedirectToRouteResult>();

            RedirectToRouteResult result = _result as RedirectToRouteResult;

            result.RouteName.Should().Be(RouteConstants.AdminSearchLearnersRecords);
            result.RouteValues.Should().ContainKey(PageNumberKey);
            result.RouteValues[PageNumberKey].Should().Be(_adminSearchLearnerViewModel.SearchLearnerCriteria.PageNumber);
        }
    }
}