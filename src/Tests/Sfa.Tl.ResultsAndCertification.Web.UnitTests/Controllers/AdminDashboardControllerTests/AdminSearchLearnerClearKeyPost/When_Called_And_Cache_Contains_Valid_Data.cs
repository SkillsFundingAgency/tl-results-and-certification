using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnerClearKeyPost
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
                    PageNumber = PageNumber
                }
            };

            CacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey).Returns(_viewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnerClearKeyAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminSearchLearnerViewModel>(
                p => p.SearchLearnerCriteria.SearchKey == string.Empty
                && p.SearchLearnerCriteria.PageNumber == PageNumber));
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            _viewModel.SearchLearnerCriteria.Should().NotBeNull();
            _viewModel.SearchLearnerCriteria.SearchKey.Should().BeEmpty();
            _viewModel.SearchLearnerCriteria.PageNumber.Should().Be(PageNumber);
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