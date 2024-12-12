using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminNotificationControllerTests.AdminNotificationDetailsGet
{
    public class When_NotificationDetails_Found : AdminNotificationControllerBaseTest
    {
        private const int NotificationId = 1;

        private readonly AdminNotificationDetailsViewModel _viewModel = new()
        {
            NotificationId = NotificationId,
            Title = "notification title",
            Content = "notification content",
            SummaryTarget = new()
            {
                Id = "target",
                Title = "Target",
                Value = "Awarding organisation"
            },
            SummaryStartDate = new()
            {
                Id = "startdate",
                Title = "Start date",
                Value = "01 Jan 2024"
            },
            SummaryEndDate = new()
            {
                Id = "enddate",
                Title = "End date",
                Value = "02 Jan 2024"
            }
        };

        private readonly NotificationBannerModel _banner = new AdminNotificationBannerModel(AdminEditNotification.Message_Notification_Updated);

        public override void Given()
        {
            AdminNotificationLoader.GetNotificationDetailsViewModel(NotificationId).Returns(_viewModel);
            CacheService.GetAndRemoveAsync<NotificationBannerModel>(NotificationCacheKey).Returns(_banner);
        }

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminNotificationDetailsAsync(NotificationId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminNotificationLoader.GetNotificationDetailsViewModel(NotificationId);
            CacheService.Received(1).GetAndRemoveAsync<NotificationBannerModel>(NotificationCacheKey);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            AdminNotificationDetailsViewModel _expectedViewModel = new()
            {
                NotificationId = _viewModel.NotificationId,
                Title = _viewModel.Title,
                Content = _viewModel.Content,
                SummaryTarget = _viewModel.SummaryTarget,
                SummaryStartDate = _viewModel.SummaryStartDate,
                SummaryEndDate = _viewModel.SummaryEndDate,
                SuccessBanner = _banner,
            };

            var viewModel = _result.ShouldBeViewResult<AdminNotificationDetailsViewModel>();
            viewModel.Should().BeEquivalentTo(_expectedViewModel);

            viewModel.BackLink.RouteName.Should().Be(RouteConstants.AdminFindNotification);
            viewModel.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}