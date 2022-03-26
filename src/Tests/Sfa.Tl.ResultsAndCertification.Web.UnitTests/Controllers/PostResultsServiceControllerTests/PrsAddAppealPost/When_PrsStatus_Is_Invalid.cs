using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradePost
{
    public class When_PrsStatus_Is_Invalid : TestSetup
    {
        private PrsAddAppealViewModel _mockLoaderResponse = null;

        public override void Given()
        {
            _mockLoaderResponse = new PrsAddAppealViewModel
            {
                //ProfileId = ProfileId,
                //PathwayAssessmentId = AssessmentId,
                //PathwayResultId = 9,
                //Uln = 1234567890,
                //LearnerName = "John Smith",
                //DateofBirth = DateTime.Today.AddYears(-20),
                //PathwayCode = "12121212",
                //PathwayName = "Childcare",
                //PathwayAssessmentSeries = "Summer 2021",
                //PathwayGrade = "A",
                //PathwayPrsStatus = PrsStatus.BeingAppealed
            };

            //ViewModel = new PrsAddAppealViewModel { ProfileId = 1, PathwayAssessmentId = 11, AppealGrade = true };
            //Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId)
            //    .Returns(_mockLoaderResponse);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            //var routeName = (Result as RedirectToRouteResult).RouteName;
            //routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
