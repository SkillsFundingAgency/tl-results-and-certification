using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminaddProviderPost
{
    public class When_Request_Invalid : AdminProviderControllerBaseTest
    {
        private readonly AdminAddProviderViewModel _viewModel = new()
        {
            UkPrn = "10000528",
            Name = "Barking & Dagenham College",
            DisplayName = "Barking & Dagenham College"
        };

        private IActionResult _result;

        public override void Given()
        {
            AddProviderResponse addResponse = new()
            {
                ProviderId = 0,
                Success = false,
                DuplicatedUkprnFound = true,
                DuplicatedNameFound = true,
                DuplicatedDisplayNameFound = true
            };

            AdminProviderLoader.SubmitAddProviderRequest(_viewModel).Returns(addResponse);
        }

        public async override Task When()
        {
            _result = await Controller.AdminAddProviderAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminProviderLoader.Received(1).SubmitAddProviderRequest(_viewModel);
            CacheService.DidNotReceive().SetAsync(NotificationCacheKey, Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Controller.ModelState.ErrorCount.Should().Be(3);
            Controller.ModelState.Should().ContainKey("Ukprn");
            Controller.ModelState.Should().ContainKey("Name");
            Controller.ModelState.Should().ContainKey("DisplayName");

            var resultViewModel = _result.ShouldBeViewResult<AdminAddProviderViewModel>();
            resultViewModel.Should().Be(_viewModel);
        }
    }
}