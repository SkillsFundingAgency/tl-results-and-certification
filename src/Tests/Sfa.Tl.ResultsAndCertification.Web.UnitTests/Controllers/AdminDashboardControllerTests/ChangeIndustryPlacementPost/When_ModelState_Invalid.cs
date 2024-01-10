using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        protected AdminChangeIndustryPlacementViewModel MockResult = null;

        public override void Given()
        {
            AdminChangeIndustryPlacementViewModel = new AdminChangeIndustryPlacementViewModel()
            {
                PathwayId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                Uln = 1100000001,
                ProviderName = "provider-name",
                ProviderUkprn = 10000536,
                TlevelName = "t-level-name",
                AcademicYear = 2022,
                DisplayAcademicYear = "2021 to 2022",
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };

            MockResult = new AdminChangeIndustryPlacementViewModel()
            {
                PathwayId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                Uln = 1100000001,
                ProviderName = "provider-name",
                ProviderUkprn = 10000536,
                TlevelName = "t-level-name",
                AcademicYear = 2022,
                DisplayAcademicYear = "2021 to 2022",
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeIndustryPlacementViewModel>(Arg.Any<int>()).Returns(MockResult);

            Controller.ModelState.AddModelError(nameof(AdminChangeIndustryPlacementViewModel.IndustryPlacementStatus), Content.AdminDashboard.AdminChangeIndustryPlacement.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AdminChangeIndustryPlacementViewModel));

            var model = viewResult.Model as AdminChangeIndustryPlacementViewModel;

            model.Should().NotBeNull();
            model.PathwayId.Should().Be(AdminChangeIndustryPlacementViewModel.PathwayId);
            model.FirstName.Should().Be(AdminChangeIndustryPlacementViewModel.FirstName);
            model.LastName.Should().Be(AdminChangeIndustryPlacementViewModel.LastName);
            model.Uln.Should().Be(AdminChangeIndustryPlacementViewModel.Uln);
            model.ProviderName.Should().Be(AdminChangeIndustryPlacementViewModel.ProviderName);
            model.ProviderUkprn.Should().Be(AdminChangeIndustryPlacementViewModel.ProviderUkprn);
            model.TlevelName.Should().Be(AdminChangeIndustryPlacementViewModel.TlevelName);
            model.AcademicYear.Should().Be(AdminChangeIndustryPlacementViewModel.AcademicYear);
            model.IndustryPlacementStatus.Should().Be(AdminChangeIndustryPlacementViewModel.IndustryPlacementStatus);
            model.DisplayAcademicYear.Should().Be(AdminChangeIndustryPlacementViewModel.DisplayAcademicYear);

            Controller.ViewData.ModelState.Should().HaveCount(1);
            Controller.ViewData.ModelState.ContainsKey(nameof(AdminChangeIndustryPlacementViewModel.IndustryPlacementStatus)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(AdminChangeIndustryPlacementViewModel.IndustryPlacementStatus)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.AdminDashboard.AdminChangeIndustryPlacement.Validation_Message);
        }
    }
}
