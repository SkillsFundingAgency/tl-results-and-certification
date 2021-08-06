using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsGradeChangeRequestGet
{
    public class When_PrsStatus_IsNotFinal : TestSetup
    {
        private PrsGradeChangeRequestViewModel _mockGradeChangeRequestViewModel;

        public override void Given()
        {
            ProfileId = 11;
            AssessmentId = 1;
            ResultId = 1;

            _mockGradeChangeRequestViewModel = new PrsGradeChangeRequestViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                ResultId = ResultId,
                Status = RegistrationPathwayStatus.Active,
                PathwayPrsStatus = PrsStatus.BeingAppealed
            };

            Loader.GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_mockGradeChangeRequestViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
