using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DashboardLoaderTests.GetDashboardViewModelTests
{
    public class When_Admin_Returns_Expected : TestSetup
    {
        private readonly LoginUserType _loginUserType = LoginUserType.Admin;

        public override void Given()
        {
            var claims = new[]
            {
                new Claim(CustomClaimTypes.HasAccessToService, "true"),
                new Claim(CustomClaimTypes.LoginUserType, ((int)_loginUserType).ToString())
            };

            ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.DidNotReceive().GetAwardingOrganisationBanners();
            ApiClient.DidNotReceive().GetProviderBanners();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.HasAccessToService.Should().BeTrue();
            ActualResult.LoginUserType.Should().Be(_loginUserType);
            ActualResult.Banners.Should().BeEmpty();
        }
    }
}