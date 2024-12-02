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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminAddProviderPost
{
    public class When_ModelState_Invalid : AdminProviderControllerBaseTest
    {
        private readonly AdminAddProviderViewModel _viewModel = new()
        {
            UkPrn = string.Empty,
            Name = string.Empty,
            DisplayName = string.Empty
        };

        private IActionResult _result;

        public override void Given()
        {
            Controller.ModelState.AddModelError("Ukprn", AdminAddProvider.Validation_Message_Ukprn_Required);
        }

        public async override Task When()
        {
            _result = await Controller.AdminAddProviderAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminProviderLoader.DidNotReceive().SubmitAddProviderRequest(_viewModel);
            CacheService.DidNotReceive().SetAsync(NotificationCacheKey, Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Controller.ModelState.ErrorCount.Should().Be(1);
            Controller.ModelState.Should().ContainKey("Ukprn");

            var resultViewModel = _result.ShouldBeViewResult<AdminAddProviderViewModel>();
            resultViewModel.Should().Be(_viewModel);
        }
    }
}