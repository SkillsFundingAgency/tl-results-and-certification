using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeStartYearPost
{
    public class When_Failed : TestSetup
    {
        private ReviewChangeStartYearViewModel MockResult = null;
        private NotificationBannerModel _expectedNotificationBannerModel;
        public override void Given()
        {
            var isSuccess = false;
            ReviewChangeStartYearViewModel = new ReviewChangeStartYearViewModel()
            {
                RegistrationPathwayId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                Uln = 1100000001,
                ProviderName = "provider-name",
                ProviderUkprn = 10000536,
                TlevelName = "t-level-name",
                AcademicYear = 2022,
                AcademicYearTo = "2022",
                DisplayAcademicYear = "2022 to 2023",
                ContactName = "contact-name",
                ChangeReason = "change-reason",
                Day = "01",
                Month = "01",
                Year = "1970"
            };

           
            AdminDashboardLoader.ProcessChangeStartYearAsync(ReviewChangeStartYearViewModel).Returns(isSuccess = false);

        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).ProcessChangeStartYearAsync(ReviewChangeStartYearViewModel);
           
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.ProblemWithService);
            route.RouteValues.Should().BeNullOrEmpty();

        }
    }
}
