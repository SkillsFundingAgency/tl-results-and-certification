using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminBannerControllerTests.AdminAddBannerPost
{
    public class When_ModelState_Invalid : AdminBannerControllerBaseTest
    {
        private readonly AdminAddBannerViewModel _viewModel = new()
        {
            Title = "",
            Content = "content",
            Target = BannerTarget.NotSpecified,
            StartDay = "10",
            StartMonth = "12",
            StartYear = "2024",
            EndDay = "31",
            EndMonth = "12",
            EndYear = "2024"
        };

        private IActionResult _result;

        public override void Given()
        {
        }

        public async override Task When()
        {
            _result = await Controller.AdminAddBannerAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminBannerLoader.DidNotReceive().SubmitAddBannerRequest(_viewModel);
            CacheService.DidNotReceive().SetAsync(NotificationCacheKey, Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Controller.ModelState.ErrorCount.Should().Be(2);
            Controller.ModelState.Should().ContainKey("Title");
            Controller.ModelState.Should().ContainKey("Target");

            var resultViewModel = _result.ShouldBeViewResult<AdminAddBannerViewModel>();
            resultViewModel.Should().Be(_viewModel);

            resultViewModel.BackLink.RouteName.Should().Be(RouteConstants.AdminFindBanner);
            resultViewModel.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}