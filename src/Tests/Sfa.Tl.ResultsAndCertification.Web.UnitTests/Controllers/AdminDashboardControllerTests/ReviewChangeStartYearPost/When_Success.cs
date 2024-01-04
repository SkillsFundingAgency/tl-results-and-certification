using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ReviewChangeStartYearPost
{
    public class When_Success : TestSetup
    {
        private ReviewChangeStartYearViewModel MockResult = null;
        public override void Given()
        {
            ReviewChangeStartYearViewModel = new ReviewChangeStartYearViewModel()
            {
                PathwayId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                Uln = 1100000001,
                ProviderName = "provider-name",
                ProviderUkprn = 10000536,
                TlevelName = "t-level-name",
                AcademicYear = 2022,
                AcademicYearTo = "2021",
                DisplayAcademicYear = "2021 to 2022",
                ContactName = "contact-name",
                ChangeReason = "change-reason",
                Day = "01",
                Month = "01",
                Year = "1970"
            };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(Arg.Any<int>());
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(p => p.Message == LearnerRecord.Message_Notification_Success), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_AdminLearnerRecord()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(nameof(RouteConstants.AdminLearnerRecord));
            route.RouteValues[Constants.PathwayId].Should().Be(ReviewChangeStartYearViewModel.PathwayId);
        }
    }
}