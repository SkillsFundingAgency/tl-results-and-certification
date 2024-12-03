using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminBannerControllerTests.AdminAddBannerPost
{
    public class When_Request_Unsuccessful : AdminBannerControllerBaseTest
    {
        private readonly AdminAddBannerViewModel _viewModel = new()
        {
            Title = "title",
            Content = "content",
            Target = BannerTarget.Both,
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
            AddBannerResponse response = new()
            {
                Success = false
            };

            AdminBannerLoader.SubmitAddBannerRequest(_viewModel).Returns(response);
        }

        public async override Task When()
        {
            _result = await Controller.AdminAddBannerAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminBannerLoader.SubmitAddBannerRequest(_viewModel);
            CacheService.DidNotReceive().SetAsync(NotificationCacheKey, Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToProblemWithService();
        }
    }
}