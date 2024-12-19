using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests.AdminAddNotificationPost
{
    public class When_Request_Unsuccessful : AdminNotificationControllerBaseTest
    {
        private readonly AdminAddNotificationViewModel _viewModel = new()
        {
            Title = "title",
            Content = "content",
            Target = NotificationTarget.Both,
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
            AddNotificationResponse response = new()
            {
                Success = false
            };

            AdminNotificationLoader.SubmitAddNotificationRequest(_viewModel).Returns(response);
        }

        public async override Task When()
        {
            _result = await Controller.AdminAddNotificationAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminNotificationLoader.SubmitAddNotificationRequest(_viewModel);
            CacheService.DidNotReceive().SetAsync(NotificationCacheKey, Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToProblemWithService();
        }
    }
}