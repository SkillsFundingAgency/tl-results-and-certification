using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelAppealUpdatePost
{
    public class When_AreYouSureToCancel_IsFalse : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsCancelAppealUpdateViewModel { ProfileId = 1, AreYouSureToCancel = false };
        }

        [Fact]
        public void Then_Redirected_To_PrsAppealCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsAppealCheckAndSubmit);
            route.RouteValues.Should().BeNull();
        }
    }
}
