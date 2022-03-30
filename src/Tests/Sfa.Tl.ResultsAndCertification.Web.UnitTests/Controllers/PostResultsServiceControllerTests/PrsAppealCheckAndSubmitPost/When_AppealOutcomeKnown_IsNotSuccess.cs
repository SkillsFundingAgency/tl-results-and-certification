using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCheckAndSubmitPost
{
    public class When_AppealOutcomeKnown_IsNotSuccess : TestSetup
    {
        public override void Given()
        {
            var isAppealSuccess = false;

            Loader.PrsAppealActivityAsync(AoUkprn, Arg.Any<PrsAppealCheckAndSubmitViewModel>()).Returns(isAppealSuccess);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            Loader.Received(1).PrsAppealActivityAsync(AoUkprn, Arg.Any<PrsAppealCheckAndSubmitViewModel>());
        }
    }
}
