using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsGradeChangeRequestConfirmationPost
{
    public class When_Called_With_No_Option : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsGradeChangeRequestConfirmationViewModel { ProfileId = 1, AssessmentId = 10, NavigationOption = null };
        }

        [Fact]
        public void Then_Redirected_To_Home()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.Home);
            route.RouteValues.Should().BeNull();
        }
    }
}
