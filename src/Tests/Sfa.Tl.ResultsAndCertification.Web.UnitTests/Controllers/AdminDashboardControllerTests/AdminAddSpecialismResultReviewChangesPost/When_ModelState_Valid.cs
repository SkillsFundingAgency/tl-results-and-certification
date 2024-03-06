using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminAddSpecialismResultReviewChangesPost
{
    public class When_ModelState_Valid : TestSetup
    {
        private readonly AdminAddSpecialismResultReviewChangesViewModel _viewModel = CreateViewModel();

        public override void Given()
        {
            AdminDashboardLoader.ProcessAddSpecialismResultReviewChangesAsync(_viewModel).Returns(true);
        }

        public async override Task When()
        {
            Result = await Controller.AdminAddSpecialismResultReviewChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).ProcessAddSpecialismResultReviewChangesAsync(_viewModel);
            CacheService.Received(1).SetAsync(
                CacheKey,
                Arg.Is<NotificationBannerModel>(p => p.Message.Contains(AdminAddSpecialismResultReviewChanges.Notification_Message_Asessment_Result_Added)),
                CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToActionResult(RouteConstants.AdminLearnerRecord, (Constants.PathwayId, _viewModel.RegistrationPathwayId));
        }
    }
}