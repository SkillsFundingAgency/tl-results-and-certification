using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ReviewChangeStartYearPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        protected ReviewChangeStartYearViewModel MockResult = null;

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
                ContactName = "contact-name"
            };

            MockResult = new ReviewChangeStartYearViewModel()
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
                ContactName = "contact-name"
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(Arg.Any<int>()).Returns(MockResult);

            Controller.ModelState.AddModelError(nameof(ReviewChangeStartYearViewModel.ContactName), Content.AdminDashboard.ReviewChangeStartYear.Validation_Contact_Name_Blank_Text);
            Controller.ModelState.AddModelError(nameof(ReviewChangeStartYearViewModel.RequestDate), Content.AdminDashboard.ReviewChangeStartYear.Validation_Date_When_Change_Requested_Blank_Text);
            Controller.ModelState.AddModelError(nameof(ReviewChangeStartYearViewModel.ChangeReason), Content.AdminDashboard.ReviewChangeStartYear.Validation_Reason_For_Change_Blank_Text);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReviewChangeStartYearViewModel));

            var model = viewResult.Model as ReviewChangeStartYearViewModel;

            model.Should().NotBeNull();
            model.PathwayId.Should().Be(ReviewChangeStartYearViewModel.PathwayId);
            model.FirstName.Should().Be(ReviewChangeStartYearViewModel.FirstName);
            model.LastName.Should().Be(ReviewChangeStartYearViewModel.LastName);
            model.Uln.Should().Be(ReviewChangeStartYearViewModel.Uln);
            model.ProviderName.Should().Be(ReviewChangeStartYearViewModel.ProviderName);
            model.ProviderUkprn.Should().Be(ReviewChangeStartYearViewModel.ProviderUkprn);
            model.TlevelName.Should().Be(ReviewChangeStartYearViewModel.TlevelName);
            model.AcademicYear.Should().Be(ReviewChangeStartYearViewModel.AcademicYear);
            model.AcademicYearTo.Should().Be(ReviewChangeStartYearViewModel.AcademicYearTo);
            model.DisplayAcademicYear.Should().Be(ReviewChangeStartYearViewModel.DisplayAcademicYear);

            Controller.ViewData.ModelState.Should().HaveCount(3);
            Controller.ViewData.ModelState.ContainsKey(nameof(ReviewChangeStartYearViewModel.ContactName)).Should().BeTrue();
            Controller.ViewData.ModelState.ContainsKey(nameof(ReviewChangeStartYearViewModel.RequestDate)).Should().BeTrue();
            Controller.ViewData.ModelState.ContainsKey(nameof(ReviewChangeStartYearViewModel.ChangeReason)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ReviewChangeStartYearViewModel.ContactName)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.AdminDashboard.ReviewChangeStartYear.Validation_Contact_Name_Blank_Text);
        }
    }
}
