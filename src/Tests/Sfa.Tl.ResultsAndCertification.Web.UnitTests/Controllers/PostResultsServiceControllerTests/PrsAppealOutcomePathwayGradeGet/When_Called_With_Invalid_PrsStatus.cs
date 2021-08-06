using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealOutcomePathwayGradeGet
{
    public class When_Called_With_Invalid_PrsStatus : TestSetup
    {
        private AppealOutcomePathwayGradeViewModel _appealOutcomerPathwayGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;

            _appealOutcomerPathwayGradeViewModel = new AppealOutcomePathwayGradeViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                PathwayResultId = 9,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                PathwayCode = "12121212",
                PathwayName = "Childcare",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayGrade = "A",
                PathwayPrsStatus = null
            };

            Loader.GetPrsLearnerDetailsAsync<AppealOutcomePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealOutcomerPathwayGradeViewModel);
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
