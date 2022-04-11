using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelRommUpdatePost
{
    public class When_AreYouSureToCancel_IsFalse : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsCancelRommUpdateViewModel { ProfileId = 1, AreYouSureToCancel= false };
        }

        [Fact]
        public void Then_Redirected_To_PrsRommCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsRommCheckAndSubmit);
            route.RouteValues.Should().BeNull();
        }
    }
}
