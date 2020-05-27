using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderPost
{
    public class Then_On_Valid_ViewModel_Post_Redirected_To_SelectToProviderTlevels_Route : When_FindProviderAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            ProviderLoader.HasAnyTlevelSetupForProviderAsync(Ukprn, SelectedProviderId).Returns(false);
        }

        [Fact]
        public void Then_Redirected_ToSelectProviderTLevels()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SelectProviderTlevels);
        }
    }
}
