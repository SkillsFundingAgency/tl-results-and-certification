﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelChangeGradeRequestPost
{
    public class When_PrsJourney_With_Option_No : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsCancelGradeChangeRequestViewModel 
            { 
                ProfileId = 1, 
                AssessmentId = 10, 
                ComponentType = Common.Enum.ComponentType.Core,
                AreYouSureToCancel = false,
                IsResultJourney = false
            };
        }

        [Fact]
        public void Then_Redirected_To_PrsGradeChangeRequest()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsGradeChangeRequest);
            route.RouteValues.Count.Should().Be(3);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.AssessmentId);
            route.RouteValues[Constants.ComponentType].Should().Be((int)ViewModel.ComponentType);
        }
    }
}
