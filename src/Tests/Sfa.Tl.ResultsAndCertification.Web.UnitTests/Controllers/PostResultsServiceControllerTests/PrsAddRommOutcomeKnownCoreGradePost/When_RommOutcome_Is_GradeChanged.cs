using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownCoreGradePost
{
    public class When_RommOutcome_Is_GradeChanged : TestSetup
    {
        private PrsAddRommOutcomeKnownCoreGradeViewModel _addRommOutcomeKnownCoreGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;

            _addRommOutcomeKnownCoreGradeViewModel = new PrsAddRommOutcomeKnownCoreGradeViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = " Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "TLevel in Childcare",
                CoreDisplayName = "Childcare (12121212)",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                PrsStatus = null,
                RommEndDate = DateTime.UtcNow.AddDays(7)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core).Returns(_addRommOutcomeKnownCoreGradeViewModel);
            ViewModel = new PrsAddRommOutcomeKnownCoreGradeViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, RommOutcome = RommOutcomeKnownType.GradeChanged };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PrsRommGradeChange()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsRommGradeChange);
            route.RouteValues.Count.Should().Be(2);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.AssessmentId);
        }
    }
}
