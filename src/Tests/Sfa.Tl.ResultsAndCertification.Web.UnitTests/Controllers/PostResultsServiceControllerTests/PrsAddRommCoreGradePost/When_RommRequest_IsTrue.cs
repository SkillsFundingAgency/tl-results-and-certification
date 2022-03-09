using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommCoreGradePost
{
    public class When_RommRequest_IsTrue : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsAddRommCoreGradeViewModel { ProfileId = 1, AssessmentId = 11, IsRommRequested = true, RommEndDate = DateTime.Today.AddDays(7) };
            Loader.GetPrsLearnerDetailsAsync<PrsAddRommCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core).Returns(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PrsAddRommOutcomeKnownCoreGrade()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsAddRommOutcomeKnownCoreGrade);
            route.RouteValues.Count.Should().Be(2);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.AssessmentId);
        }
    }
}
