using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminProviderDetailsGet
{
    public class When_ProviderDetails_Found : AdminProviderControllerBaseTest
    {
        private const int ProviderId = 1;

        private readonly AdminProviderDetailsViewModel _viewModel = new()
        {
            ProviderId = ProviderId,
            ProviderName = "Barnsley College"
        };

        private readonly NotificationBannerModel _banner = new AdminNotificationBannerModel(AdminEditProvider.Notification_Message_Provider_Updated);

        public override void Given()
        {
            AdminProviderLoader.GetProviderDetailsViewModel(ProviderId).Returns(_viewModel);
            CacheService.GetAndRemoveAsync<NotificationBannerModel>(NotificationCacheKey).Returns(_banner);
        }

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminProviderDetailsAsync(ProviderId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminProviderLoader.GetProviderDetailsViewModel(ProviderId);
            CacheService.Received(1).GetAndRemoveAsync<NotificationBannerModel>(NotificationCacheKey);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            AdminProviderDetailsViewModel _expectedViewModel = new()
            {
                ProviderId = _viewModel.ProviderId,
                ProviderName = _viewModel.ProviderName,
                SuccessBanner = _banner
            };

            var viewModel = _result.ShouldBeViewResult<AdminProviderDetailsViewModel>();
            viewModel.Should().BeEquivalentTo(_expectedViewModel);
        }
    }
}