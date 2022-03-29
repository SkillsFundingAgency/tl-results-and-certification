using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomeKnownPost
{
    public class When_PrsStatus_Is_Invalid_For_Core : TestSetup
    {
        private PrsAddAppealOutcomeKnownViewModel _mockLoaderResponse = null;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 10;
            ComponentType = ComponentType.Core;

            _mockLoaderResponse = new PrsAddAppealOutcomeKnownViewModel
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
                PrsStatus = PrsStatus.BeingAppealed,
                AppealOutcome = AppealOutcomeKnownType.No,
                AppealEndDate = DateTime.Today.AddDays(7)
            };

            ViewModel = new PrsAddAppealOutcomeKnownViewModel { ProfileId = 1, AssessmentId = 11, ComponentType = ComponentType, AppealOutcome = AppealOutcomeKnownType.No };
            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType)
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
