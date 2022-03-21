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
    public class When_Called_With_IsChangeMode_True_For_Core : TestSetup
    {
        private PrsRommGradeChangeViewModel _rommGradeChangeViewModel;
        private PrsRommCheckAndSubmitViewModel _prsRommCheckAndSubmitViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            IsChangeMode = true;
            ComponentType = ComponentType.Core;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _rommGradeChangeViewModel = new PrsRommGradeChangeViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                CoreName = "Childcare",
                CoreLarId = "12121212",
                ExamPeriod = "Summer 2021",
                Grade = "B",
                PrsStatus = PrsStatus.UnderReview,
                Grades = _grades,
                ComponentType = ComponentType
            };

            _prsRommCheckAndSubmitViewModel = null;
            CacheService.GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey).Returns(_prsRommCheckAndSubmitViewModel);
            Loader.GetPrsLearnerDetailsAsync<PrsRommGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_rommGradeChangeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsRommGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
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
            model.CoreName.Should().Be(_rommGradeChangeViewModel.CoreName);
            model.CoreLarId.Should().Be(_rommGradeChangeViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_rommGradeChangeViewModel.CoreName} ({_rommGradeChangeViewModel.CoreLarId})");
            model.ExamPeriod.Should().Be(_rommGradeChangeViewModel.ExamPeriod);
            model.Grade.Should().Be(_rommGradeChangeViewModel.Grade);
            model.RommEndDate.Should().Be(_rommGradeChangeViewModel.RommEndDate);
            model.ComponentType.Should().Be(_rommGradeChangeViewModel.ComponentType);
            model.SelectedGradeCode.Should().BeNull();
            model.Grades.Should().BeEquivalentTo(_rommGradeChangeViewModel.Grades);
            model.IsChangeMode.Should().BeTrue();
            model.IsValid.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsRommCheckAndSubmit);
            model.BackLink.RouteAttributes.Should().BeNull();
        }
    }
}
