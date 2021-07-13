using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradePost
{
    public class When_AppealRequest_IsFalse : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AppealCoreGradeViewModel { ProfileId = 1, PathwayAssessmentId = 11, AppealGrade = false };
            var mockLoderResponse = new AppealCoreGradeViewModel();
            Loader.GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId)
                .Returns(mockLoderResponse);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(2);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.PathwayAssessmentId);
        }
    }
}
