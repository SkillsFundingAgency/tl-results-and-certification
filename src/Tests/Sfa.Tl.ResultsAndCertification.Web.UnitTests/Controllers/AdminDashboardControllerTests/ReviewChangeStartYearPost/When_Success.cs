using NSubstitute;
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

            //MockResult = new ReviewChangeStartYearViewModel()
            //{
            //    PathwayId = 1,
            //    FirstName = "firstname",
            //    LastName = "lastname",
            //    Uln = 1100000001,
            //    ProviderName = "provider-name",
            //    ProviderUkprn = 10000536,
            //    TlevelName = "t-level-name",
            //    AcademicYear = 2022,
            //    AcademicYearTo = "2021",
            //    DisplayAcademicYear = "2021 to 2022",
            //    ContactName = "contact-name"
            //};
            
            //AdminDashboardLoader.GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(Arg.Any<int>()).Returns(MockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(Arg.Any<int>());
        }

        // Todo
        //[Fact]
        //public void Then_Redirected_To_ReviewChangeStartYear()
        //{
        //    var route = Result as RedirectToActionResult;
        //    route.ActionName.Should().Be(nameof(RouteConstants.ReviewChangeStartYear));
        //    route.RouteValues[Constants.PathwayId].Should().Be(ReviewChangeStartYearViewModel.PathwayId);
        //}
    }
}