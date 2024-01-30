using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnerRecordsApplyFiltersPost
{
    public class When_Called_And_Cache_Contains_Valid_Data_No_Provider_Search : AdminDashboardControllerTestBase
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
                Search = string.Empty
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
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnerRecordsApplyFiltersAsync(_filtersViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ProviderLoader.DidNotReceive().GetProviderLookupDataAsync(Arg.Any<string>(), Arg.Any<bool>());
            CacheService.Received(1).GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminSearchLearnerViewModel>(
                p => p.SearchLearnerCriteria.SearchKey == _criteriaViewModel.SearchKey
                && p.SearchLearnerCriteria.PageNumber == _criteriaViewModel.PageNumber
                && p.SearchLearnerCriteria.SearchLearnerFilters.Search == _filtersViewModel.Search
                && !p.SearchLearnerCriteria.SearchLearnerFilters.SelectedProviderId.HasValue));
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            AdminSearchLearnerCriteriaViewModel criteria = _adminSearchLearnerViewModel.SearchLearnerCriteria;
            AdminSearchLearnerFiltersViewModel filters = criteria.SearchLearnerFilters;

            criteria.Should().NotBeNull();
            criteria.SearchKey.Should().Be(_criteriaViewModel.SearchKey);
            criteria.PageNumber.Should().Be(_criteriaViewModel.PageNumber);

            filters.Search.Should().BeEmpty();
            filters.SelectedProviderId.Should().BeNull();
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