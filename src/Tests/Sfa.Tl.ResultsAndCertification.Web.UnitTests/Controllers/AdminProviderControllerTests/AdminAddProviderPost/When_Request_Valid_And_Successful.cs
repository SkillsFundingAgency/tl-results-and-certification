using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminAddProviderPost
{
    public class When_Request_Valid_And_Successful : AdminProviderControllerBaseTest
    {
        private const int ProviderId = 1;

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
                ProviderId = ProviderId,
                Success = true,
                DuplicatedUkprnFound = false,
                DuplicatedNameFound = false
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

            CacheService.Received(1).SetAsync(NotificationCacheKey,
                Arg.Is<NotificationBannerModel>(n => n.Message.Contains(AdminAddProvider.Notification_Message_Provider_Added)),
                Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminProviderDetails, ("providerId", ProviderId));
        }
    }
}