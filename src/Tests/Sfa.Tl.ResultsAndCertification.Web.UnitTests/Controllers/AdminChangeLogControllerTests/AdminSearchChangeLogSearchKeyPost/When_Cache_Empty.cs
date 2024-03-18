using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminChangeLogControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchChangeLogSearchKeyPost
{
    public class When_Cache_Empty : AdminChangeLogControllerTestBase
    {
        private readonly AdminSearchChangeLogCriteriaViewModel _viewModel = new()
        {
            SearchKey = "smith",
            PageNumber = 1
        };

        private IActionResult _result;

        public override void Given()
        {
            CacheService.GetAsync<AdminSearchChangeLogViewModel>(CacheKey).ReturnsNull();
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchChangeLogSearchKeyAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchChangeLogViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<AdminSearchChangeLogViewModel>());

            AdminChangeLogLoader.ReceivedCalls().Should().BeEmpty();
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            _result.ShouldBeRedirectPageNotFound();
        }
    }
}