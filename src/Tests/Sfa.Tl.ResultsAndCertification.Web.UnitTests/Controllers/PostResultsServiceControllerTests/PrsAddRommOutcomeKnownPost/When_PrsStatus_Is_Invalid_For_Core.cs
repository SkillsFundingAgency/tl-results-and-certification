using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownPost
{
    public class When_PrsStatus_Is_Invalid_For_Core : TestSetup
    {
        private PrsAddRommOutcomeKnownViewModel _mockLoaderResponse = null;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 10;
            ComponentType = ComponentType.Core;

            _mockLoaderResponse = new PrsAddRommOutcomeKnownViewModel
            {
                ProfileId = 1,
                AssessmentId = 11,
                ResultId = 17,
                Firstname = "Test",
                Lastname = "John",
                ExamPeriod = "Summer 2022",
                CoreDisplayName = "Education (1234567)",
                ComponentType = ComponentType,
                PrsStatus = PrsStatus.UnderReview,
                RommOutcome = RommOutcomeKnownType.No,
                RommEndDate = DateTime.Today.AddDays(7)
            };

            ViewModel = new PrsAddRommOutcomeKnownViewModel { ProfileId = 1, AssessmentId = 11, ComponentType = ComponentType, RommOutcome = RommOutcomeKnownType.No };
            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType)
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
