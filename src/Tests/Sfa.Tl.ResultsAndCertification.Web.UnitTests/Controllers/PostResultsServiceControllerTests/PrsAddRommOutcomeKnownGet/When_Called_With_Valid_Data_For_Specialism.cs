using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownGet
{
    public class When_Called_With_Valid_Data_For_Specialism : TestSetup
    {
        private PrsAddRommOutcomeKnownViewModel _addRommOutcomeKnownViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Specialism;

            _addRommOutcomeKnownViewModel = new PrsAddRommOutcomeKnownViewModel
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
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addRommOutcomeKnownViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAddRommOutcomeKnownViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addRommOutcomeKnownViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addRommOutcomeKnownViewModel.AssessmentId);
            model.Uln.Should().Be(_addRommOutcomeKnownViewModel.Uln);
            model.LearnerName.Should().Be(_addRommOutcomeKnownViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addRommOutcomeKnownViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addRommOutcomeKnownViewModel.TlevelTitle);
            model.CoreDisplayName.Should().Be(_addRommOutcomeKnownViewModel.CoreDisplayName);
            model.ExamPeriod.Should().Be(_addRommOutcomeKnownViewModel.ExamPeriod);
            model.Grade.Should().Be(_addRommOutcomeKnownViewModel.Grade);
            model.RommEndDate.Should().Be(_addRommOutcomeKnownViewModel.RommEndDate);
            model.ComponentType.Should().Be(_addRommOutcomeKnownViewModel.ComponentType);
            model.RommOutcome.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddRomm);
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
