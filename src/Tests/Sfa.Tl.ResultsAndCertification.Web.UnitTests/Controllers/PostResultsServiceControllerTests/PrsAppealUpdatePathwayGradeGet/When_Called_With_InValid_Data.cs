using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealUpdatePathwayGradeGet
{
    public class When_Called_With_InValid_Data : TestSetup
    {
        private AppealUpdatePathwayGradeViewModel _appealUpdatePathwayGradeViewModel;

        public override void Given()
        {
            ProfileId = 0;
            AssessmentId = 7;
            ResultId = 9;

            _appealUpdatePathwayGradeViewModel = null;

            Loader.GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealUpdatePathwayGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
