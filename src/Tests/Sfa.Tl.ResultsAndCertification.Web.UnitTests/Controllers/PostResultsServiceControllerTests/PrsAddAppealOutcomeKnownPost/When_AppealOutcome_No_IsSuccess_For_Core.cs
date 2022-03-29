using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using PrsAddAppealOutcomeKnownContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddAppealOutcomeKnown;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomeKnownPost
{
    public class When_AppealOutcome_No_IsSuccess_For_Core : TestSetup
    {
        private readonly bool _prsActivityResponse = true;
        private string _expectedSuccessBannerMsg;
        private string _expectedHeaderMsg;

        public override void Given()
        {
            ComponentType = ComponentType.Core;
            ViewModel = new PrsAddAppealOutcomeKnownViewModel
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
                AppealOutcome = AppealOutcomeKnownType.No,
                AppealEndDate = DateTime.Today.AddDays(7),
                PrsStatus = PrsStatus.Reviewed
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType)
                .Returns(ViewModel);

            Loader.PrsAppealActivityAsync(AoUkprn, ViewModel).Returns(_prsActivityResponse);

            _expectedHeaderMsg = PrsAddAppealOutcomeKnownContent.Banner_HeaderMessage_Appeal_Recorded;
            _expectedSuccessBannerMsg = string.Format(PrsAddAppealOutcomeKnownContent.Banner_Message, ViewModel.LearnerName, ViewModel.ExamPeriod, ViewModel.CoreDisplayName);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType);
            Loader.Received(1).PrsAppealActivityAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.IsPrsJourney == true && x.HeaderMessage.Equals(_expectedHeaderMsg) && x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
