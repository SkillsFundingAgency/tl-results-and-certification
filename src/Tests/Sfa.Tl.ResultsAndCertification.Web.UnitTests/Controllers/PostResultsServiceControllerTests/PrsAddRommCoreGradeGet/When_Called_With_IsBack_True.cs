using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommCoreGradeGet
{
    public class When_Called_With_IsBack_True : TestSetup
    {
        private PrsAddRommCoreGradeViewModel _addRommCoreGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            IsBack = true;

            _addRommCoreGradeViewModel = new PrsAddRommCoreGradeViewModel
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

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core).Returns(_addRommCoreGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAddRommCoreGradeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addRommCoreGradeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addRommCoreGradeViewModel.AssessmentId);
            model.Uln.Should().Be(_addRommCoreGradeViewModel.Uln);
            model.LearnerName.Should().Be(_addRommCoreGradeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addRommCoreGradeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addRommCoreGradeViewModel.TlevelTitle);
            model.CoreDisplayName.Should().Be(_addRommCoreGradeViewModel.CoreDisplayName);
            model.ExamPeriod.Should().Be(_addRommCoreGradeViewModel.ExamPeriod);
            model.Grade.Should().Be(_addRommCoreGradeViewModel.Grade);
            model.RommEndDate.Should().Be(_addRommCoreGradeViewModel.RommEndDate);
            model.IsRommRequested.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
