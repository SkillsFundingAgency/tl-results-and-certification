using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.DashboardBanner;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DashboardLoaderTests.GetDashboardViewModelTests
{
    public class When_Provider_Returns_Expected : TestSetup
    {
        private readonly LoginUserType _loginUserType = LoginUserType.TrainingProvider;

        public override void Given()
        {
            var claims = new[]
            {
                new Claim(CustomClaimTypes.HasAccessToService, "true"),
                new Claim(CustomClaimTypes.LoginUserType, ((int)_loginUserType).ToString())
            };

            ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var banners = new[]
            {
                "provider_banner_content_1",
                "provider_banner_content_2"
            };

            ApiClient.GetProviderBanners().Returns(banners);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.DidNotReceive().GetAwardingOrganisationBanners();
            ApiClient.Received(1).GetProviderBanners();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.HasAccessToService.Should().BeTrue();
            ActualResult.LoginUserType.Should().Be(_loginUserType);

            var bannerModels = new[]
            {
                new DashboardBannerModel("provider_banner_content_1"),
                new DashboardBannerModel("provider_banner_content_2")
            };

            ActualResult.Banners.Should().BeEquivalentTo(bannerModels);
        }
    }
}