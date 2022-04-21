using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using PrsAddAppealOutcomeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddAppealOutcome;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomePost
{
    public class When_AppealOutcome_Withdraw_IsFailed_For_Core : TestSetup
    {
        private PrsAddAppealOutcomeViewModel _addAppealOutcomeViewModel;
        private string _expectedSuccessBannerMsg;
        private string _expectedBannerHeaderMsg;

        public override void Given()
        {
            ComponentType = ComponentType.Core;

            _addAppealOutcomeViewModel = new PrsAddAppealOutcomeViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = " Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "TLevel in Childcare",
                CoreName = "Childcare",
                CoreLarId = "12121212",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                PrsStatus = PrsStatus.BeingAppealed,
                ComponentType = ComponentType
            };

            ViewModel = new PrsAddAppealOutcomeViewModel
            {
                ProfileId = 1,
                AssessmentId = 2,
                ResultId = 3,
                AppealOutcome = AppealOutcomeType.Withdraw,
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType).Returns(_addAppealOutcomeViewModel);
            Loader.PrsAppealActivityAsync(AoUkprn, ViewModel).Returns(false);

            _expectedBannerHeaderMsg = PrsAddAppealOutcomeContent.Banner_HeaderMessage_Appeal_Withdrawn;
            _expectedSuccessBannerMsg = string.Format(PrsAddAppealOutcomeContent.Banner_Message, _addAppealOutcomeViewModel.LearnerName, _addAppealOutcomeViewModel.ExamPeriod, _addAppealOutcomeViewModel.CoreDisplayName);
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
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType);
            Loader.Received(1).PrsAppealActivityAsync(AoUkprn, ViewModel);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.DisplayMessageBody == true && x.Message.Equals(_expectedBannerHeaderMsg) && x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }
    }
}
