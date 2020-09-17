using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderPost
{
    public class When_TlevelSetupForProvider_IsFalse : TestSetup
    {
        public override void Given()
        {
            ProviderLoader.HasAnyTlevelSetupForProviderAsync(Ukprn, SelectedProviderId).Returns(false);
        }

        [Fact]
        public void Then_Redirected_To_SelectProviderTlevels()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SelectProviderTlevels);
        }
    }
}
