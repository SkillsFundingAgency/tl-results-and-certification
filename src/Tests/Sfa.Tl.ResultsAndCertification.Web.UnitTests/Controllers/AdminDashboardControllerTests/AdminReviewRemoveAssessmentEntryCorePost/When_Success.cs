using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewRemoveAssessmentEntryCorePost
{
    public class When_Success : TestSetup
    { 
        private int RegistrationPathwayId = 1;

        public override void Given()
        {
            ReviewRemoveCoreAssessmentEntryViewModel = new()
            {
                PathwayAssessmentViewModel = new()
                {
                    RegistrationPathwayId = 1
                },
                ContactName = "John Smith",
                Day = "01",
                Month = "01",
                Year = "1970",
                ChangeReason = "Reason for change",
                ZendeskId = "1234566780"
            };

            AdminDashboardLoader.ProcessRemoveAssessmentEntry(ReviewRemoveCoreAssessmentEntryViewModel).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).ProcessRemoveAssessmentEntry(ReviewRemoveCoreAssessmentEntryViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(p => p.Message == AdminReviewRemoveAssessmentEntry.Message_Notification_Success), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_AdminLearnerRecord()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(nameof(RouteConstants.AdminLearnerRecord));
            route.RouteValues[Constants.PathwayId].Should().Be(RegistrationPathwayId);
        }
    }
}