using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomePost
{
    public class When_PrsStatus_Is_Invalid_For_Core : TestSetup
    {
        private PrsAddRommOutcomeViewModel _mockLoaderResponse = null;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 10;
            ComponentType = ComponentType.Core;

            _mockLoaderResponse = new PrsAddRommOutcomeViewModel
            {
                ProfileId = 1,
                AssessmentId = 11,
                ResultId = 17,
                Firstname = "Test",
                Lastname = "John",
                ExamPeriod = "Summer 2022",
                CoreName = "Education",
                CoreLarId = "1234567",
                ComponentType = ComponentType,
                PrsStatus = null,
                RommOutcome = RommOutcomeType.GradeChanged,
                RommEndDate = DateTime.Today.AddDays(7)
            };

            ViewModel = new PrsAddRommOutcomeViewModel { ProfileId = 1, AssessmentId = 11, RommOutcome = RommOutcomeType.GradeChanged };
            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType)
                  .Returns(_mockLoaderResponse);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
