using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealPost
{
    public class When_AppealRequest_IsTrue_For_Core : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsAddAppealViewModel { ProfileId = 1, AssessmentId = 11, ComponentType = ComponentType.Core, IsAppealRequested = true, AppealEndDate = DateTime.Today.AddDays(7) };
            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core).Returns(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PrsAddAppealOutcomeKnownCoreGrade()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsAddAppealOutcomeKnown);
            route.RouteValues.Count.Should().Be(3);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.AssessmentId);
            route.RouteValues[Constants.ComponentType].Should().Be((int)ViewModel.ComponentType);
        }
    }
}
