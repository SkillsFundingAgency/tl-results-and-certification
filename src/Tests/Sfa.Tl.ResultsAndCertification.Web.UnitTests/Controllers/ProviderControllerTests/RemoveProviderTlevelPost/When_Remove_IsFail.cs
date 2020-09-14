using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelPost
{
    public class When_Remove_IsFail : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ProviderTlevelDetailsViewModel { Id = TqProviderId, TlProviderId = TlProviderId, CanRemoveTlevel = true };
            ProviderLoader.RemoveTqProviderTlevelAsync(Ukprn, TqProviderId).Returns(false);
        }

        [Fact]
        public void Then_Redirected_To_Error()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            var routeValue = (Result.Result as RedirectToRouteResult).RouteValues["StatusCode"];
            routeName.Should().Be(RouteConstants.Error);
            routeValue.Should().Be(500);
        }
    }
}
