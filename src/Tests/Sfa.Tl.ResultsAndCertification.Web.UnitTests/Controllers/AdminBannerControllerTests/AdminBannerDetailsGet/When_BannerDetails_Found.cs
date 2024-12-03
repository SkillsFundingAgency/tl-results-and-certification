using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminBannerControllerTests.AdminBannerDetailsGet
{
    public class When_BannerDetails_Found : AdminBannerControllerBaseTest
    {
        private const int BannerId = 1;

        private readonly AdminBannerDetailsViewModel _viewModel = new()
        {
            BannerId = BannerId,
            Title = "banner title",
            Content = "banner content",
            SummaryTarget = new()
            {
                Id = "target",
                Title = "Target",
                Value = "Awarding organisation"
            },
            SummaryIsActive = new()
            {
                Id = "active",
                Title = "Active",
                Value = "Yes"
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

        private readonly NotificationBannerModel _banner = new AdminNotificationBannerModel(AdminEditBanner.Notification_Message_Banner_Updated);

        public override void Given()
        {
            AdminBannerLoader.GetBannerDetailsViewModel(BannerId).Returns(_viewModel);
            CacheService.GetAndRemoveAsync<NotificationBannerModel>(NotificationCacheKey).Returns(_banner);
        }

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminBannerDetailsAsync(BannerId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminBannerLoader.GetBannerDetailsViewModel(BannerId);
            CacheService.Received(1).GetAndRemoveAsync<NotificationBannerModel>(NotificationCacheKey);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            AdminBannerDetailsViewModel _expectedViewModel = new()
            {
                BannerId = _viewModel.BannerId,
                Title = _viewModel.Title,
                Content = _viewModel.Content,
                SummaryTarget = _viewModel.SummaryTarget,
                SummaryIsActive = _viewModel.SummaryIsActive,
                SummaryStartDate = _viewModel.SummaryStartDate,
                SummaryEndDate = _viewModel.SummaryEndDate,
                SuccessBanner = _banner,
            };

            var viewModel = _result.ShouldBeViewResult<AdminBannerDetailsViewModel>();
            viewModel.Should().BeEquivalentTo(_expectedViewModel);

            viewModel.BackLink.RouteName.Should().Be(RouteConstants.AdminFindBanner);
            viewModel.BackLink.RouteAttributes.Should().BeEmpty();
        }
    }
}