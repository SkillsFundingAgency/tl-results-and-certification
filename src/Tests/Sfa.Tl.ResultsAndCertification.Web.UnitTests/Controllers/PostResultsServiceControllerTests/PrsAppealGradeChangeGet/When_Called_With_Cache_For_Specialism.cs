using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeChangeGet
{
    public class When_Called_With_Cache_For_Specialism : TestSetup
    {
        private PrsAppealGradeChangeViewModel _appealGradeChangeViewModel;
        private List<LookupViewModel> _grades;
        private PrsAppealCheckAndSubmitViewModel _checkAndSubmitCacheViewModel;
        private string _expectedSelectedGrade;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Specialism;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "Pass", Value = "V1" }, new LookupViewModel { Id = 2, Code = "MERIT", Value = "V2" } };
            _appealGradeChangeViewModel = new PrsAppealGradeChangeViewModel
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
                SpecialismName = "Ventilation",
                SpecialismLarId = "Z12345467",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                PrsStatus = PrsStatus.Reviewed,
                AppealEndDate = DateTime.UtcNow.AddDays(7),
                Grades = _grades,
                ComponentType = ComponentType
            };
            
            Loader.GetPrsLearnerDetailsAsync<PrsAppealGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_appealGradeChangeViewModel);
            
            _checkAndSubmitCacheViewModel = new PrsAppealCheckAndSubmitViewModel { NewGrade = "V1" };
            CacheService.GetAsync<PrsAppealCheckAndSubmitViewModel>(CacheKey).Returns(_checkAndSubmitCacheViewModel);
            _expectedSelectedGrade = _grades.FirstOrDefault(x => x.Value.Equals(_checkAndSubmitCacheViewModel.NewGrade, StringComparison.InvariantCultureIgnoreCase))?.Code;
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAppealGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsAppealGradeChangeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_appealGradeChangeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_appealGradeChangeViewModel.AssessmentId);
            model.Uln.Should().Be(_appealGradeChangeViewModel.Uln);
            model.LearnerName.Should().Be(_appealGradeChangeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_appealGradeChangeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_appealGradeChangeViewModel.TlevelTitle);
            model.CoreName.Should().Be(_appealGradeChangeViewModel.CoreName);
            model.CoreLarId.Should().Be(_appealGradeChangeViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_appealGradeChangeViewModel.CoreName} ({_appealGradeChangeViewModel.CoreLarId})");
            model.SpecialismName.Should().Be(_appealGradeChangeViewModel.SpecialismName);
            model.SpecialismLarId.Should().Be(_appealGradeChangeViewModel.SpecialismLarId);
            model.SpecialismDisplayName.Should().Be($"{_appealGradeChangeViewModel.SpecialismName} ({_appealGradeChangeViewModel.SpecialismLarId})");
            model.ExamPeriod.Should().Be(_appealGradeChangeViewModel.ExamPeriod);
            model.Grade.Should().Be(_appealGradeChangeViewModel.Grade);
            model.AppealEndDate.Should().Be(_appealGradeChangeViewModel.AppealEndDate);
            model.ComponentType.Should().Be(_appealGradeChangeViewModel.ComponentType);
            model.SelectedGradeCode.Should().Be(_expectedSelectedGrade);
            model.Grades.Should().BeEquivalentTo(_appealGradeChangeViewModel.Grades);
            model.IsValid.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddAppealOutcomeKnown);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            assessmentIdRouteValue.Should().Be(AssessmentId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.ComponentType, out string componentTypeRouteValue);
            componentTypeRouteValue.Should().Be(((int)ComponentType).ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AppealOutcomeKnownTypeId, out string AppealOutcomeKnownRouteValue);
            AppealOutcomeKnownRouteValue.Should().Be(((int)AppealOutcomeKnownType.GradeChanged).ToString());
        }
    }
}
