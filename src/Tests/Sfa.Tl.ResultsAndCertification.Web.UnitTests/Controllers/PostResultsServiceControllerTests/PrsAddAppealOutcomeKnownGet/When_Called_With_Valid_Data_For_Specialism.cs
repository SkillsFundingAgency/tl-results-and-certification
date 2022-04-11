using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomeKnownGet
{
    public class When_Called_With_Valid_Data_For_Specialism : TestSetup
    {
        private PrsAddAppealOutcomeKnownViewModel _addAppealOutcomeKnownViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Specialism;

            _addAppealOutcomeKnownViewModel = new PrsAddAppealOutcomeKnownViewModel
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
                SpecialismName = "Heating Engineering",
                SpecialismLarId = "Z2345678",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                PrsStatus = PrsStatus.Reviewed,
                AppealEndDate = DateTime.UtcNow.AddDays(7),
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addAppealOutcomeKnownViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAddAppealOutcomeKnownViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addAppealOutcomeKnownViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addAppealOutcomeKnownViewModel.AssessmentId);
            model.Uln.Should().Be(_addAppealOutcomeKnownViewModel.Uln);
            model.LearnerName.Should().Be(_addAppealOutcomeKnownViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addAppealOutcomeKnownViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addAppealOutcomeKnownViewModel.TlevelTitle);
            model.CoreName.Should().Be(_addAppealOutcomeKnownViewModel.CoreName);
            model.CoreLarId.Should().Be(_addAppealOutcomeKnownViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_addAppealOutcomeKnownViewModel.CoreName} ({_addAppealOutcomeKnownViewModel.CoreLarId})");
            model.SpecialismLarId.Should().Be(_addAppealOutcomeKnownViewModel.SpecialismLarId);
            model.SpecialismDisplayName.Should().Be($"{_addAppealOutcomeKnownViewModel.SpecialismName} ({_addAppealOutcomeKnownViewModel.SpecialismLarId})");
            model.ExamPeriod.Should().Be(_addAppealOutcomeKnownViewModel.ExamPeriod);
            model.Grade.Should().Be(_addAppealOutcomeKnownViewModel.Grade);
            model.AppealEndDate.Should().Be(_addAppealOutcomeKnownViewModel.AppealEndDate);
            model.ComponentType.Should().Be(_addAppealOutcomeKnownViewModel.ComponentType);
            model.AppealOutcome.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddAppeal);
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
