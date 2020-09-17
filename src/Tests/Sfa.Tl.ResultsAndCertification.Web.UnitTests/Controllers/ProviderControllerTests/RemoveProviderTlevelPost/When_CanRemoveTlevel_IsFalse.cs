using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelPost
{
    public class When_CanRemoveTlevel_IsFalse : TestSetup
    {
        public override void Given()
        {
            TlProviderId = 1;
            ViewModel = new ProviderTlevelDetailsViewModel
            {
                TlProviderId = TlProviderId,
                CanRemoveTlevel = false
            };
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.ProviderTlevels);
            route.RouteValues["providerId"].Should().Be(TlProviderId);
        }
    }
}
