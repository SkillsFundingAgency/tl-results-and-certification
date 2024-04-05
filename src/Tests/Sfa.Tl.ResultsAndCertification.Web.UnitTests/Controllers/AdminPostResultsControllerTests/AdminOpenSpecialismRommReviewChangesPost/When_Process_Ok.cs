using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismRommReviewChangesPost
{
    public class When_Process_Ok : TestSetup
    {
        private AdminOpenSpecialismRommReviewChangesViewModel _viewModel;
        private IActionResult _result;

        public override void Given()
        {
            _viewModel = CreateViewModel();
            AdminPostResultsLoader.ProcessAdminOpenSpecialismRommAsync(_viewModel).Returns(true);
        }

        public async override Task When()
        {
            _result = await Controller.AdminOpenSpecialismRommReviewChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).SetAsync<NotificationBannerModel>(
                AdminDashboardCacheKey,
                Arg.Is<AdminNotificationBannerModel>(p => p.Message.Contains(AdminOpenSpecialismRommReviewChanges.Notification_Message_A_Romm_Has_Been_Opened)),
                CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToRouteResult(RouteConstants.AdminLearnerRecord, ("pathwayId", _viewModel.RegistrationPathwayId));
        }
    }
}