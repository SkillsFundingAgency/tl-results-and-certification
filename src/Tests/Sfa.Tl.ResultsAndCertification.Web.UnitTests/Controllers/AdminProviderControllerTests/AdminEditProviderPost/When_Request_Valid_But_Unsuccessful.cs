using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminEditProviderPost
{
    public class When_Request_Valid_But_Unsuccessful : AdminProviderControllerBaseTest
    {
        private readonly AdminEditProviderViewModel _viewModel = new()
        {
            ProviderId = 1,
            UkPrn = "10000528",
            Name = "Barking & Dagenham College",
            DisplayName = "Barking & Dagenham College",
            IsActive = true
        };

        private IActionResult _result;

        public override void Given()
        {
            UpdateProviderResponse updateResponse = new()
            {
                Success = false,
                DuplicatedUkprnFound = false,
                DuplicatedNameFound = false
            };

            AdminProviderLoader.SubmitUpdateProviderRequest(_viewModel).Returns(updateResponse);
        }

        public async override Task When()
        {
            _result = await Controller.AdminEditProviderAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminProviderLoader.Received(1).SubmitUpdateProviderRequest(_viewModel);
            CacheService.DidNotReceive().SetAsync(NotificationCacheKey, Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToProblemWithService();
        }
    }
}