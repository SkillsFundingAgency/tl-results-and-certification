using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAppealCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCheckAndSubmitPost
{
    public class When_Appeal_IsSuccess_For_Core : TestSetup
    {
        private string _expectedSuccessBannerMsg;
        private string _expectedBannerHeaderMsg;

        public override void Given()
        {
            var isAppealSuccess = true;
            ViewModel = new PrsAppealCheckAndSubmitViewModel
            {
                ProfileId = 1,
                AssessmentId = 11,
                ResultId = 12,
                ComponentType = ComponentType.Core,
                Firstname = "John",
                Lastname = "Smith",
                ExamPeriod = "Summer 2022",
                CoreName = "Design and Education",
                CoreLarId = "12345678",
            };

            Loader.PrsAppealActivityAsync(AoUkprn, ViewModel).Returns(isAppealSuccess);
            _expectedSuccessBannerMsg = string.Format(CheckAndSubmitContent.Banner_Message, ViewModel.LearnerName, ViewModel.ExamPeriod, ViewModel.CoreDisplayName);
            _expectedBannerHeaderMsg = CheckAndSubmitContent.Banner_HeaderMessage_Appeal_Recorded;
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            Loader.Received(1).PrsAppealActivityAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.DisplayMessageBody == true && x.HeaderMessage.Equals(_expectedBannerHeaderMsg) && x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }
    }
}
