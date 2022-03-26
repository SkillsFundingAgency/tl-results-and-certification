using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradePost
{
    public class When_AppealRequest_IsFalse_For_Core : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsAddAppealViewModel { ProfileId = 1, AssessmentId = 11, ComponentType = ComponentType.Core, IsAppealRequested = false, AppealEndDate = DateTime.Today.AddDays(7) };
            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType.Core)
                .Returns(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
