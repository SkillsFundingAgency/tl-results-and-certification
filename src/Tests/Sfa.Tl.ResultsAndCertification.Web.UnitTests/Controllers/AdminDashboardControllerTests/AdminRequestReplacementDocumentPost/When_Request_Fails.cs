using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminRequestReplacementDocumentPost
{
    public class When_Request_Fails : TestSetup
    {
        public override void Given()
        {
            AdminDashboardLoader
                .CreateReplacementDocumentPrintingRequestAsync(ViewModel)
                .Returns(false);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<AdminNotificationBannerModel>(), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.ProblemWithService);
        }
    }
}