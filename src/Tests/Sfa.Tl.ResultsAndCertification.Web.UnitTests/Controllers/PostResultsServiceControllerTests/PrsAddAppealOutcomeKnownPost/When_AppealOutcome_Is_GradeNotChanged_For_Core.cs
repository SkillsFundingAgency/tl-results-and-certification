using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomeKnownPost
{
    public class When_AppealOutcome_Is_GradeNotChanged_For_Core : TestSetup
    {
        private PrsAppealCheckAndSubmitViewModel _checkAndSubmitViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 10;
            ComponentType = ComponentType.Core;

            ViewModel = new PrsAddAppealOutcomeKnownViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                AppealEndDate = DateTime.UtcNow.AddDays(10),
                AppealOutcome = AppealOutcomeKnownType.GradeNotChanged,
                ComponentType = ComponentType,
                PrsStatus = PrsStatus.Reviewed
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(ViewModel);

            _checkAndSubmitViewModel = new PrsAppealCheckAndSubmitViewModel { OldGrade = "B" };
            Loader.GetPrsLearnerDetailsAsync<PrsAppealCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_checkAndSubmitViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAppealCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<PrsAppealCheckAndSubmitViewModel>(x => ComponentType == ComponentType && x.OldGrade == x.NewGrade && x.IsGradeChanged == false));
        }

        [Fact]
        public void Then_Redirected_To_PrsAppealCheckAndSubmit()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PrsAppealCheckAndSubmit);
        }
    }
}
