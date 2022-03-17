using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using PrsAddRommOutcomeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddRommOutcome;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomePost
{
    public class When_RommOutcome_Withdraw : TestSetup
    {
        private PrsAddRommOutcomeViewModel _addRommOutcomeViewModel;
        private string _expectedSuccessBannerMsg;
        private string _expectedBannerHeaderMsg;

        public override void Given()
        {
            _addRommOutcomeViewModel = new PrsAddRommOutcomeViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = " Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "TLevel in Childcare",
                CoreDisplayName = "Childcare (12121212)",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                PrsStatus = PrsStatus.UnderReview,
                RommEndDate = DateTime.UtcNow.AddDays(7),
                ComponentType = ComponentType.Core
            };

            ViewModel = new PrsAddRommOutcomeViewModel
            {
                ProfileId = 1,
                AssessmentId = 2,
                ResultId = 3,
                RommOutcome = RommOutcomeType.Withdraw,
                ComponentType = ComponentType.Core
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ViewModel.ComponentType).Returns(_addRommOutcomeViewModel);
            Loader.PrsRommActivityAsync(AoUkprn, ViewModel).Returns(true);

            _expectedBannerHeaderMsg = PrsAddRommOutcomeContent.Banner_HeaderMessage_Romm_Withdrawn;
            _expectedSuccessBannerMsg = string.Format(PrsAddRommOutcomeContent.Banner_Message, _addRommOutcomeViewModel.LearnerName, _addRommOutcomeViewModel.ExamPeriod, _addRommOutcomeViewModel.CoreDisplayName);
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
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType.Core);
            Loader.Received(1).PrsRommActivityAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.HeaderMessage.Equals(_expectedBannerHeaderMsg) && x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }
    }
}
