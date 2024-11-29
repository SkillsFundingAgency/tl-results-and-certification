using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System.Security.Claims;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DashboardLoaderTests.GetDashboardViewModelTests
{
    public class When_NoAccess_To_Service_Returns_False : TestSetup
    {
        public override void Given()
        {
            var claim = new Claim(CustomClaimTypes.HasAccessToService, "false");
            ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.HasAccessToService.Should().BeFalse();
            ActualResult.LoginUserType.Should().Be(LoginUserType.NotSpecified);
            ActualResult.Banners.Should().BeNull();
        }
    }
}
