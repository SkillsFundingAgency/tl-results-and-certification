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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealUpdatePathwayGradeGet
{
    public class When_Called_With_Invalid_AppealsEndDate : TestSetup
    {
        private AppealUpdatePathwayGradeViewModel _appealUpdatePathwayGradeViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;
            ResultsAndCertificationConfiguration.AppealsEndDate = DateTime.UtcNow.AddDays(-7);

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _appealUpdatePathwayGradeViewModel = new AppealUpdatePathwayGradeViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                PathwayResultId = 9,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                PathwayName = "Childcare",
                PathwayCode = "12121212",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayGrade = "A",
                PathwayPrsStatus = PrsStatus.BeingAppealed,
                Grades = _grades
            };

            Loader.GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealUpdatePathwayGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
