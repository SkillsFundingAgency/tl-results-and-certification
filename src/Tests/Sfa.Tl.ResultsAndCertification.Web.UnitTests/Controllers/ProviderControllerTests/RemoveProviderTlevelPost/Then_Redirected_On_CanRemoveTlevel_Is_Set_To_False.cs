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
    public class Then_Redirected_On_CanRemoveTlevel_Is_Set_To_False : When_RemoveProviderTlevelAsync_Post_Action_Is_Called
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
        public void Then_On_Set_CanRemoveTlevel_To_False_Redirect_To_ProviderTlevels_Route()
        {
            var route = (Result.Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.ProviderTlevels);

            route.RouteValues["providerId"].Should().Be(TlProviderId);
        }
    }
}
