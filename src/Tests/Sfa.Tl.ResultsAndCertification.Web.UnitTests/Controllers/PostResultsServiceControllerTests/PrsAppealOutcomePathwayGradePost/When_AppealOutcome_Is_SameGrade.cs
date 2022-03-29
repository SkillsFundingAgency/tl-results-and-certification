using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealOutcomePathwayGradePost
{
    public class When_AppealOutcome_Is_SameGrade : TestSetup
    {
        private PrsPathwayGradeCheckAndSubmitViewModel _checkAndSubmitViewModel;

        public override void Given()
        {
            ViewModel = new AppealOutcomePathwayGradeViewModel { AppealOutcome = AppealOutcomeType.GradeNotChanged };

            _checkAndSubmitViewModel = new PrsPathwayGradeCheckAndSubmitViewModel { OldGrade = "B" };
            Loader.GetPrsLearnerDetailsAsync<PrsPathwayGradeCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_checkAndSubmitViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey);
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsPathwayGradeCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<PrsPathwayGradeCheckAndSubmitViewModel>(x => x.OldGrade == x.NewGrade && x.IsGradeChanged == false));
        }

        [Fact]
        public void Then_Redirected_To_PrsPathwayGradeCheckAndSubmit()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PrsPathwayGradeCheckAndSubmit);
        }
    }
}
