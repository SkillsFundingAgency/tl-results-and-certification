using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        protected AdminIpCompletionViewModel MockResult = null;

        public override void Given()
        {
            AdminChangeIndustryPlacementViewModel = new AdminIpCompletionViewModel
            {
                RegistrationPathwayId = 1,
                LearnerName = "firstname lastname",
                Uln = 1100000001,
                Provider = "provider-name (10000536)",
                TlevelName = "t-level-name",
                AcademicYear = 2022,
                StartYear = "2021 to 2022",
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };

            MockResult = new AdminIpCompletionViewModel()
            {
                RegistrationPathwayId = 1,
                LearnerName = "firstname lastname",
                Uln = 1100000001,
                Provider = "provider-name (10000536)",
                TlevelName = "t-level-name",
                AcademicYear = 2022,
                StartYear = "2021 to 2022",
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(Arg.Any<int>()).Returns(MockResult);

            Controller.ModelState.AddModelError(nameof(AdminChangeIndustryPlacementViewModel.IndustryPlacementStatus), Content.AdminDashboard.AdminChangeIndustryPlacement.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AdminIpCompletionViewModel));

            var model = viewResult.Model as AdminIpCompletionViewModel;

            model.Should().NotBeNull();
            model.RegistrationPathwayId.Should().Be(AdminChangeIndustryPlacementViewModel.RegistrationPathwayId);
            model.LearnerName.Should().Be(AdminChangeIndustryPlacementViewModel.LearnerName);
            model.Uln.Should().Be(AdminChangeIndustryPlacementViewModel.Uln);
            model.Provider.Should().Be(AdminChangeIndustryPlacementViewModel.Provider);
            model.TlevelName.Should().Be(AdminChangeIndustryPlacementViewModel.TlevelName);
            model.AcademicYear.Should().Be(AdminChangeIndustryPlacementViewModel.AcademicYear);
            model.IndustryPlacementStatus.Should().Be(AdminChangeIndustryPlacementViewModel.IndustryPlacementStatus);
            model.StartYear.Should().Be(AdminChangeIndustryPlacementViewModel.StartYear);

            Controller.ViewData.ModelState.Should().HaveCount(1);
            Controller.ViewData.ModelState.ContainsKey(nameof(AdminChangeIndustryPlacementViewModel.IndustryPlacementStatus)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(AdminChangeIndustryPlacementViewModel.IndustryPlacementStatus)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.AdminDashboard.AdminChangeIndustryPlacement.Validation_Message);
        }
    }
}
