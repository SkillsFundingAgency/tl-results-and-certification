using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewRemoveAssessmentEntryCorePost
{
    public class When_Failed : TestSetup
    {
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

            AdminDashboardLoader.ProcessRemoveAssessmentEntry(ReviewRemoveCoreAssessmentEntryViewModel).Returns(false);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).ProcessRemoveAssessmentEntry(ReviewRemoveCoreAssessmentEntryViewModel);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<NotificationBannerModel>());
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(RouteConstants.ProblemWithService);
            route.RouteValues.Should().BeNullOrEmpty();

        }
    }
}
