using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomeKnownPost
{
    public class When_AppealOutcome_No_IsNotSuccess_For_Core : TestSetup
    {
        private readonly bool _prsActivityResponse = false;

        public override void Given()
        {
            ComponentType = ComponentType.Core;
            ViewModel = new PrsAddAppealOutcomeKnownViewModel 
            {
                ProfileId = 1,
                AssessmentId = 11,
                ResultId = 17,
                ComponentType = ComponentType,
                AppealOutcome = AppealOutcomeKnownType.No,
                AppealEndDate = DateTime.Today.AddDays(7),
                PrsStatus = PrsStatus.Reviewed
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType)
                .Returns(ViewModel);

            Loader.PrsAppealActivityAsync(AoUkprn, ViewModel).Returns(_prsActivityResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType);
            Loader.Received(1).PrsAppealActivityAsync(AoUkprn, ViewModel);
            CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
