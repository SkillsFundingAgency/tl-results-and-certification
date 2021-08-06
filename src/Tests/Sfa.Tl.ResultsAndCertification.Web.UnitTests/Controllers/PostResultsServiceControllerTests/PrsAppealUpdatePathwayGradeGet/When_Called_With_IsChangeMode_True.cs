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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealUpdatePathwayGradeGet
{
    public class When_Called_With_IsChangeMode_True : TestSetup
    {
        private AppealUpdatePathwayGradeViewModel _appealUpdatePathwayGradeViewModel;
        private PrsPathwayGradeCheckAndSubmitViewModel _prsCheckAndSubmitViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;
            IsChangeMode = true;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _appealUpdatePathwayGradeViewModel = new AppealUpdatePathwayGradeViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                PathwayResultId = ResultId,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                PathwayCode = "12121212",
                PathwayName = "Childcare",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayGrade = "B",
                PathwayPrsStatus = PrsStatus.BeingAppealed,
                Grades = _grades
            };

            _prsCheckAndSubmitViewModel = new PrsPathwayGradeCheckAndSubmitViewModel { NewGrade = "V2" };
            CacheService.GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey).Returns(_prsCheckAndSubmitViewModel);
            Loader.GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealUpdatePathwayGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey);
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AppealUpdatePathwayGradeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_appealUpdatePathwayGradeViewModel.ProfileId);
            model.PathwayAssessmentId.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayAssessmentId);
            model.PathwayResultId.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayResultId);
            model.Uln.Should().Be(_appealUpdatePathwayGradeViewModel.Uln);
            model.LearnerName.Should().Be(_appealUpdatePathwayGradeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_appealUpdatePathwayGradeViewModel.DateofBirth);
            model.PathwayName.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayName);
            model.PathwayCode.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayCode);
            model.PathwayDisplayName.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayAssessmentSeries);
            model.PathwayGrade.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayGrade);
            model.PathwayPrsStatus.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayPrsStatus);
            model.SelectedGradeCode.Should().BeNull();
            model.Grades.Should().BeEquivalentTo(_appealUpdatePathwayGradeViewModel.Grades);
            model.IsChangeMode.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsPathwayGradeCheckAndSubmit);
            model.BackLink.RouteAttributes.Should().BeNull();
        }
    }
}