﻿using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests.AdminEditNotificationPost
{
    public class When_Request_Successful : AdminNotificationControllerBaseTest
    {
        private const int NotificationId = 1;

        private readonly AdminEditNotificationViewModel _viewModel = new()
        {
            NotificationId = NotificationId,
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
            AdminNotificationLoader.SubmitUpdateNotificationRequest(_viewModel).Returns(true);
        }

        public async override Task When()
        {
            _result = await Controller.AdminEditNotificationAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminNotificationLoader.SubmitUpdateNotificationRequest(_viewModel);

            CacheService.Received(1).SetAsync(NotificationCacheKey,
                Arg.Is<NotificationBannerModel>(n => n.Message.Contains(AdminEditNotification.Message_Notification_Updated)),
                Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminNotificationDetails, (Constants.NotificationId, NotificationId));
        }
    }
}