using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownCoreGradeGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private PrsAddRommOutcomeKnownCoreGradeViewModel _addRommOutcomeKnownCoreGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

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
                RommEndDate = DateTime.UtcNow.AddDays(7),
                ComponentType = ComponentType.Core
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addRommOutcomeKnownCoreGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAddRommOutcomeKnownCoreGradeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.AssessmentId);
            model.Uln.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.Uln);
            model.LearnerName.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.TlevelTitle);
            model.CoreDisplayName.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.CoreDisplayName);
            model.ExamPeriod.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.ExamPeriod);
            model.Grade.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.Grade);
            model.RommEndDate.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.RommEndDate);
            model.ComponentType.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.ComponentType);
            model.RommOutcome.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddRommCoreGrade);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            assessmentIdRouteValue.Should().Be(AssessmentId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.ComponentType, out string componentTypeRouteValue);
            componentTypeRouteValue.Should().Be(((int)ComponentType).ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.IsBack, out string backRouteValue);
            backRouteValue.Should().Be("true");
        }
    }
}
