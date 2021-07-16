using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelAppealUpdatePost
{
    public class When_CancelRequest_IsFalse : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsCancelAppealUpdateViewModel { ProfileId = 1, AssessmentId = 10, CancelRequest = false };
        }

        [Fact]
        public void Then_Redirected_To_AddLearnerRecordCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.AddLearnerRecordCheckAndSubmit);
            route.RouteValues.Should().BeNull();
        }
    }
}
