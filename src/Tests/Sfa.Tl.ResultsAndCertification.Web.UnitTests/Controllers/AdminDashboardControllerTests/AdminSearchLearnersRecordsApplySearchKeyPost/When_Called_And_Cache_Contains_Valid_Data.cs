using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnersRecordsApplySearchKeyPost
{
    public class When_Called_And_Cache_Contains_Valid_Data : AdminDashboardControllerTestBase
    {
        private const string PageNumberKey = "pageNumber";
        private const int PageNumber = 1;

        private AdminSearchLearnerCriteriaViewModel _criteriaViewModel;
        private AdminSearchLearnerViewModel _adminSearchLearnerViewModel;
        private IActionResult _result;

        public override void Given()
        {
            _criteriaViewModel = new AdminSearchLearnerCriteriaViewModel
            {
                SearchKey = "johnson",
                PageNumber = PageNumber
            };

            _adminSearchLearnerViewModel = new AdminSearchLearnerViewModel();

            CacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey).Returns(_adminSearchLearnerViewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnersRecordsApplySearchKeyAsync(_criteriaViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminSearchLearnerViewModel>(
                p => p.SearchLearnerCriteria.SearchKey == _criteriaViewModel.SearchKey
                && p.SearchLearnerCriteria.PageNumber == _criteriaViewModel.PageNumber));
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            _adminSearchLearnerViewModel.SearchLearnerCriteria.Should().NotBeNull();
            _adminSearchLearnerViewModel.SearchLearnerCriteria.SearchKey.Should().Be(_criteriaViewModel.SearchKey);
            _adminSearchLearnerViewModel.SearchLearnerCriteria.PageNumber.Should().Be(_criteriaViewModel.PageNumber);
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