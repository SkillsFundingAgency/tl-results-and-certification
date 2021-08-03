using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelChangeGraderRequestPost
{
    public class When_ResultJourney_With_Option_Yes : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsCancelGradeChangeRequestViewModel 
            { 
                ProfileId = 1, 
                AssessmentId = 10, 
                AreYouSureToCancel = true,
                IsResultJourney = true
            };
        }

        [Fact]
        public void Then_Redirected_To_ResultDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.ResultDetails);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
