using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using WithdrawAppealContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsWithdrawAppeal;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsWithdrawAppealTests
{
    public class When_WithdrawAppeal_IsSuccess : TestSetup
    {
        private string _expectedSuccessBannerMsg;

        public override void Given()
        {
            var isAppealSuccess = true;
            ViewModel = new AppealOutcomePathwayGradeViewModel
            {
                ProfileId = 1,
                PathwayAssessmentId = 2,
                PathwayResultId = 3,
                AppealOutcome = AppealOutcomeType.WithdrawAppeal,
                PathwayCode = "12345678",
                PathwayName = "Design, Suervey and Planning"
            };

            Loader.WithdrawAppealCoreGradeAsync(AoUkprn, ViewModel).Returns(isAppealSuccess);
            _expectedSuccessBannerMsg = string.Format(WithdrawAppealContent.Success_Banner_Message, ViewModel.PathwayName, ViewModel.PathwayCode);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(2);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.PathwayAssessmentId);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            Loader.Received(1).WithdrawAppealCoreGradeAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }
    }
}
