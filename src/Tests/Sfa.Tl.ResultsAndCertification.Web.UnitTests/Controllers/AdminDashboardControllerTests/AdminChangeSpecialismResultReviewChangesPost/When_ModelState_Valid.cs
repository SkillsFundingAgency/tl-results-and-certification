using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangeSpecialismResultReviewChangesPost
{
    public class When_ModelState_Valid : TestSetup
    {
        private readonly AdminChangeSpecialismResultReviewChangesViewModel _viewModel = CreateViewModel();

        public override void Given()
        {
            AdminDashboardLoader.ProcessChangeSpecialismResultReviewChangesAsync(_viewModel).Returns(true);
        }

        public async override Task When()
        {
            Result = await Controller.AdminChangeSpecialismResultReviewChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).ProcessChangeSpecialismResultReviewChangesAsync(_viewModel);
            CacheService.Received(1).SetAsync(
                CacheKey,
                Arg.Is<NotificationBannerModel>(p => p.Message.Contains(AdminChangeSpecialismResultReviewChanges.Notification_Message_Asessment_Result_Updated)),
                CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToActionResult(RouteConstants.AdminLearnerRecord, (Constants.PathwayId, _viewModel.RegistrationPathwayId));
        }
    }
}