using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminBannerControllerTests.AdminEditBannerPost
{
    public class When_Request_Successful : AdminBannerControllerBaseTest
    {
        private const int BannerId = 1;

        private readonly AdminEditBannerViewModel _viewModel = new()
        {
            BannerId = BannerId,
            Title = "title",
            Content = "content",
            Target = BannerTarget.Both,
            IsActive = true,
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
            AdminBannerLoader.SubmitUpdateBannerRequest(_viewModel).Returns(true);
        }

        public async override Task When()
        {
            _result = await Controller.AdminEditBannerAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminBannerLoader.SubmitUpdateBannerRequest(_viewModel);

            CacheService.Received(1).SetAsync(NotificationCacheKey,
                Arg.Is<NotificationBannerModel>(n => n.Message.Contains(AdminEditBanner.Notification_Message_Banner_Updated)),
                Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminBannerDetails, ("bannerId", BannerId));
        }
    }
}