using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminEditProviderPost
{
    public class When_ModelState_Invalid : AdminProviderControllerBaseTest
    {
        private readonly AdminEditProviderViewModel _viewModel = new()
        {
            ProviderId = 1,
            UkPrn = "",
            Name = "",
            DisplayName = "",
            IsActive = true
        };

        private IActionResult _result;

        public override void Given()
        {
            Controller.ModelState.AddModelError("Ukprn", AdminEditProvider.Validation_Message_Ukprn_Required);
        }

        public async override Task When()
        {
            _result = await Controller.AdminEditProviderAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminProviderLoader.DidNotReceive().SubmitUpdateProviderRequest(_viewModel);
            CacheService.DidNotReceive().SetAsync(NotificationCacheKey, Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Controller.ModelState.ErrorCount.Should().Be(1);
            Controller.ModelState.Should().ContainKey("Ukprn");

            var resultViewModel = _result.ShouldBeViewResult<AdminEditProviderViewModel>();
            resultViewModel.Should().Be(_viewModel);
        }
    }
}