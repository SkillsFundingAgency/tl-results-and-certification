using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminBannerControllerTests.AdminEditBannerGet
{
    public class When_BannerDetails_Found : AdminBannerControllerBaseTest
    {
        private const int BannerId = 1;

        private readonly AdminEditBannerViewModel _viewModel = new()
        {
            BannerId = BannerId,
            Title = "title",
            Content = "content",
            IsActive = true,
            StartDay = "10",
            StartMonth = "12",
            StartYear = "2024",
            EndDay = "31",
            EndMonth = "12",
            EndYear = "2024"
        };

        public override void Given()
        {
            AdminBannerLoader.GetEditBannerViewModel(BannerId).Returns(_viewModel);
        }

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminEditBannerAsync(BannerId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminBannerLoader.GetEditBannerViewModel(BannerId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var viewModel = _result.ShouldBeViewResult<AdminEditBannerViewModel>();
            viewModel.Should().Be(_viewModel);

            viewModel.BackLink.RouteName.Should().Be(RouteConstants.AdminBannerDetails);
            viewModel.BackLink.RouteAttributes.Should().Contain("bannerId", BannerId.ToString());
        }
    }
}