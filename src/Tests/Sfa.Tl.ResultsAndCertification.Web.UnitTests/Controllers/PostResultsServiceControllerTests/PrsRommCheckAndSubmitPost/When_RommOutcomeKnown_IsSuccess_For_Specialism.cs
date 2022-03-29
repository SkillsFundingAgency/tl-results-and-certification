using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsRommCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsRommCheckAndSubmitPost
{
    public class When_RommOutcomeKnown_IsSuccess_For_Specialism : TestSetup
    {
        private string _expectedSuccessBannerMsg;
        private string _expectedBannerHeaderMsg;

        public override void Given()
        {
            var isRommSuccess = true;
            ViewModel = new PrsRommCheckAndSubmitViewModel
            {
                ProfileId = 1,
                AssessmentId = 11,
                ResultId = 12,
                ComponentType = ComponentType.Specialism,
                Firstname = "John",
                Lastname = "Smith",
                ExamPeriod = "Summer 2022",
                SpecialismName = "Design and Education",
                SpecialismLarId = "12345678",
            };

            Loader.PrsRommActivityAsync(AoUkprn, ViewModel).Returns(isRommSuccess);
            _expectedSuccessBannerMsg = string.Format(CheckAndSubmitContent.Banner_Message, ViewModel.LearnerName, ViewModel.ExamPeriod, ViewModel.SpecialismDisplayName);
            _expectedBannerHeaderMsg = CheckAndSubmitContent.Banner_HeaderMessage_Romm_Recorded;
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
            Loader.Received(1).PrsRommActivityAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.IsPrsJourney == true && x.HeaderMessage.Equals(_expectedBannerHeaderMsg) && x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }
    }
}
