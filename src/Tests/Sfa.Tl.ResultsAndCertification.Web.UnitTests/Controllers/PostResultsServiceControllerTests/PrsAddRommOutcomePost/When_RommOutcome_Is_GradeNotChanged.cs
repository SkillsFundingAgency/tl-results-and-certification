using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomePost
{
    public class When_RommOutcome_Is_GradeNotChanged : TestSetup
    {
        private PrsRommCheckAndSubmitViewModel _checkAndSubmitViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 10;

            ViewModel = new PrsAddRommOutcomeViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                RommEndDate = DateTime.UtcNow.AddDays(10),
                RommOutcome = RommOutcomeType.GradeNotChanged,
                PrsStatus = PrsStatus.UnderReview
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core).Returns(ViewModel);

            _checkAndSubmitViewModel = new PrsRommCheckAndSubmitViewModel { OldGrade = "B" };
            Loader.GetPrsLearnerDetailsAsync<PrsRommCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core).Returns(_checkAndSubmitViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core);
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsRommCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<PrsRommCheckAndSubmitViewModel>(x => x.OldGrade == x.NewGrade && x.IsGradeChanged == false));
        }

        [Fact]
        public void Then_Redirected_To_PrsRommCheckAndSubmit()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PrsRommCheckAndSubmit);
        }
    }
}
