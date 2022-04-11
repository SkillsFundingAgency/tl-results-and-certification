using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomePost
{
    public class When_AppealOutcome_Is_GradeChanged_For_Core : TestSetup
    {
        private PrsAddAppealOutcomeViewModel _addAppealOutcomeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _addAppealOutcomeViewModel = new PrsAddAppealOutcomeViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = " Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "TLevel in Childcare",
                CoreName = "Childcare",
                CoreLarId = "12121212",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                PrsStatus = PrsStatus.BeingAppealed,
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addAppealOutcomeViewModel);
            ViewModel = new PrsAddAppealOutcomeViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, ComponentType = ComponentType, AppealOutcome = AppealOutcomeType.GradeChanged };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<PrsAppealCheckAndSubmitViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PrsAppealGradeChange()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsAppealGradeChange);
            route.RouteValues.Count.Should().Be(4);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.AssessmentId);
            route.RouteValues[Constants.ComponentType].Should().Be((int)ViewModel.ComponentType);
            route.RouteValues[Constants.IsAppealOutcomeJourney].Should().Be("true");
        }
    }
}
