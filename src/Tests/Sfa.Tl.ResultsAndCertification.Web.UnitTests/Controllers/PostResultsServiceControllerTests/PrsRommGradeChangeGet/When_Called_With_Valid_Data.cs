using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsRommGradeChangeGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private PrsRommGradeChangeViewModel _rommGradeChangeViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _rommGradeChangeViewModel = new PrsRommGradeChangeViewModel
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
                Grades = _grades,
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsRommGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_rommGradeChangeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsRommGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
            CacheService.Received(1).GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsRommGradeChangeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_rommGradeChangeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_rommGradeChangeViewModel.AssessmentId);
            model.Uln.Should().Be(_rommGradeChangeViewModel.Uln);
            model.LearnerName.Should().Be(_rommGradeChangeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_rommGradeChangeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_rommGradeChangeViewModel.TlevelTitle);
            model.CoreDisplayName.Should().Be(_rommGradeChangeViewModel.CoreDisplayName);
            model.ExamPeriod.Should().Be(_rommGradeChangeViewModel.ExamPeriod);
            model.Grade.Should().Be(_rommGradeChangeViewModel.Grade);
            model.RommEndDate.Should().Be(_rommGradeChangeViewModel.RommEndDate);
            model.ComponentType.Should().Be(_rommGradeChangeViewModel.ComponentType);
            model.SelectedGradeCode.Should().BeNull();
            model.Grades.Should().BeEquivalentTo(_rommGradeChangeViewModel.Grades);
            model.IsChangeMode.Should().BeFalse();
            model.IsValid.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddRommOutcomeKnownCoreGrade);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            assessmentIdRouteValue.Should().Be(AssessmentId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.ComponentType, out string componentTypeRouteValue);
            componentTypeRouteValue.Should().Be(((int)ComponentType).ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.RommOutcomeKnownTypeId, out string rommOutcomeKnownRouteValue);
            rommOutcomeKnownRouteValue.Should().Be(((int)RommOutcomeKnownType.GradeChanged).ToString());
        }
    }
}
