using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomeGet
{
    public class When_Called_With_Valid_Data_For_Specialism : TestSetup
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
                SpecialismName = "Heating Engineering",
                SpecialismLarId = "Z1234567",
                Grade = "A",
                PrsStatus = PrsStatus.BeingAppealed,
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addAppealOutcomeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAddAppealOutcomeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addAppealOutcomeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addAppealOutcomeViewModel.AssessmentId);
            model.Uln.Should().Be(_addAppealOutcomeViewModel.Uln);
            model.LearnerName.Should().Be(_addAppealOutcomeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addAppealOutcomeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addAppealOutcomeViewModel.TlevelTitle);
            model.CoreName.Should().Be(_addAppealOutcomeViewModel.CoreName);
            model.CoreLarId.Should().Be(_addAppealOutcomeViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_addAppealOutcomeViewModel.CoreName} ({_addAppealOutcomeViewModel.CoreLarId})");
            model.SpecialismLarId.Should().Be(_addAppealOutcomeViewModel.SpecialismLarId);
            model.SpecialismDisplayName.Should().Be($"{_addAppealOutcomeViewModel.SpecialismName} ({_addAppealOutcomeViewModel.SpecialismLarId})");
            model.ExamPeriod.Should().Be(_addAppealOutcomeViewModel.ExamPeriod);
            model.Grade.Should().Be(_addAppealOutcomeViewModel.Grade);
            model.ComponentType.Should().Be(_addAppealOutcomeViewModel.ComponentType);
            model.AppealOutcome.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
