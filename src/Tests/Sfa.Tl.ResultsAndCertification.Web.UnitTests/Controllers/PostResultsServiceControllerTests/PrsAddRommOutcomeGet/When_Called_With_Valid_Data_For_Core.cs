using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeGet
{
    public class When_Called_With_Valid_Data_For_Core : TestSetup
    {
        private PrsAddRommOutcomeViewModel _addRommOutcomeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _addRommOutcomeViewModel = new PrsAddRommOutcomeViewModel
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
                PrsStatus = PrsStatus.UnderReview,
                RommEndDate = DateTime.UtcNow.AddDays(7)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addRommOutcomeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAddRommOutcomeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addRommOutcomeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addRommOutcomeViewModel.AssessmentId);
            model.Uln.Should().Be(_addRommOutcomeViewModel.Uln);
            model.LearnerName.Should().Be(_addRommOutcomeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addRommOutcomeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addRommOutcomeViewModel.TlevelTitle);
            model.CoreName.Should().Be(_addRommOutcomeViewModel.CoreName);
            model.CoreLarId.Should().Be(_addRommOutcomeViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_addRommOutcomeViewModel.CoreName} ({_addRommOutcomeViewModel.CoreLarId})");
            model.ExamPeriod.Should().Be(_addRommOutcomeViewModel.ExamPeriod);
            model.Grade.Should().Be(_addRommOutcomeViewModel.Grade);
            model.RommEndDate.Should().Be(_addRommOutcomeViewModel.RommEndDate);
            model.ComponentType.Should().Be(_addRommOutcomeViewModel.ComponentType);
            model.RommOutcome.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
