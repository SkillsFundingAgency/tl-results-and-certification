using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradeGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private PrsAddAppealViewModel _addAppealViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _addAppealViewModel = new PrsAddAppealViewModel
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
                PrsStatus = PrsStatus.Reviewed,
                ComponentType = ComponentType,
                AppealEndDate = DateTime.UtcNow.AddDays(7)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addAppealViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAddAppealViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addAppealViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addAppealViewModel.AssessmentId);
            model.Uln.Should().Be(_addAppealViewModel.Uln);
            model.LearnerName.Should().Be(_addAppealViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addAppealViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addAppealViewModel.TlevelTitle);
            model.CoreDisplayName.Should().Be($"{_addAppealViewModel.CoreName} ({_addAppealViewModel.CoreLarId})");
            model.ExamPeriod.Should().Be(_addAppealViewModel.ExamPeriod);
            model.Grade.Should().Be(_addAppealViewModel.Grade);
            model.AppealEndDate.Should().Be(_addAppealViewModel.AppealEndDate);
            model.ComponentType.Should().Be(_addAppealViewModel.ComponentType);
            model.IsAppealRequested.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}