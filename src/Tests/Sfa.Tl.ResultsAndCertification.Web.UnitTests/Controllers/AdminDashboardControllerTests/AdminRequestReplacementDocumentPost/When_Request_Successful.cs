using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminRequestReplacementDocumentPost
{
    public class When_Request_Successful : TestSetup
    {
        public override void Given()
        {
            AdminDashboardLoader
                .CreateReplacementDocumentPrintingRequestAsync(ViewModel)
                .Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).SetAsync(
                CacheKey,
                Arg.Is<NotificationBannerModel>(p => p.Message.Contains(AdminRequestReplacementDocument.Success_Header_Replacement_Document_Requested)),
                CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.ShouldBeRedirectToActionResult(RouteConstants.AdminLearnerRecord, (Constants.PathwayId, ViewModel.RegistrationPathwayId));
        }
    }
}