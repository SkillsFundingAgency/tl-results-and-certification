using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommPost
{
    public class When_RommRequest_IsFalse_For_Core : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsAddRommViewModel { ProfileId = 1, AssessmentId = 11, ComponentType = ComponentType.Core, IsRommRequested = false, RommEndDate = DateTime.Today.AddDays(7) };
            Loader.GetPrsLearnerDetailsAsync<PrsAddRommViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType.Core).Returns(ViewModel);
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
