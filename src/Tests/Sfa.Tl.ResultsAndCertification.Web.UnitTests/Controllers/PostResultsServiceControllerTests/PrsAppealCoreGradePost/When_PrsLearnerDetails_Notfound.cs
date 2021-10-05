using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradePost
{
    public class When_PrsLearnerDetails_Notfound : TestSetup
    {
        private readonly AppealCoreGradeViewModel _mockLoderResponse = null;

        public override void Given()
        {
            ViewModel = new AppealCoreGradeViewModel { ProfileId = 1, PathwayAssessmentId = 11, AppealGrade = false };
            Loader.GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId)
                .Returns(_mockLoderResponse);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
