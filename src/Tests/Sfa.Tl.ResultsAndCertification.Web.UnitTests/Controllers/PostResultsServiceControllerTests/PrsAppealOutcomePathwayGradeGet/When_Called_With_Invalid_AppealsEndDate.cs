using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealOutcomePathwayGradeGet
{
    public class When_Called_With_Invalid_AppealsEndDate : TestSetup
    {
        private AppealOutcomePathwayGradeViewModel _appealOutcompePathwayGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;
            ResultsAndCertificationConfiguration.AppealsEndDate = DateTime.UtcNow.AddDays(-7);

            _appealOutcompePathwayGradeViewModel = new AppealOutcomePathwayGradeViewModel
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
                PathwayPrsStatus = PrsStatus.BeingAppealed
            };

            Loader.GetPrsLearnerDetailsAsync<AppealOutcomePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealOutcompePathwayGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealOutcomePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
