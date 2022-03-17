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
    public class When_RommOutcome_Withdraw_IsFailed : TestSetup
    {
        private PrsAddRommOutcomeViewModel _addRommOutcomeViewModel;
        private string _expectedSuccessBannerMsg;
        private string _expectedBannerHeaderMsg;

        public override void Given()
        {
            ComponentType = ComponentType.Core;

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
                ComponentType = ComponentType
            };

            ViewModel = new PrsAddRommOutcomeViewModel
            {
                ProfileId = 1,
                AssessmentId = 2,
                ResultId = 3,
                RommOutcome = RommOutcomeType.Withdraw,
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType).Returns(_addRommOutcomeViewModel);
            Loader.PrsRommActivityAsync(AoUkprn, ViewModel).Returns(false);

            _expectedBannerHeaderMsg = PrsAddRommOutcomeContent.Banner_HeaderMessage_Romm_Withdrawn;
            _expectedSuccessBannerMsg = string.Format(PrsAddRommOutcomeContent.Banner_Message, _addRommOutcomeViewModel.LearnerName, _addRommOutcomeViewModel.ExamPeriod, _addRommOutcomeViewModel.CoreDisplayName);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType);
            Loader.Received(1).PrsRommActivityAsync(AoUkprn, ViewModel);            
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.IsPrsJourney == true && x.Message.Equals(_expectedBannerHeaderMsg) && x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }
    }
}
