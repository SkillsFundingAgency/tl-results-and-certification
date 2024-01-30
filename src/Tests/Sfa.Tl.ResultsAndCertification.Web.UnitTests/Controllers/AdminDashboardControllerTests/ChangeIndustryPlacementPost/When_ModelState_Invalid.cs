using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        protected AdminIpCompletionViewModel MockResult = null;

        public override void Given()
        {
            ViewModel = CreateViewModel(null as IndustryPlacementStatus?);
            Controller.ModelState.AddModelError(nameof(ViewModel.IndustryPlacementStatus), AdminChangeIndustryPlacement.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = Result.ShouldBeViewResult<AdminIpCompletionViewModel>();

            model.Should().NotBeNull();
            model.RegistrationPathwayId.Should().Be(ViewModel.RegistrationPathwayId);
            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.Uln.Should().Be(ViewModel.Uln);
            model.Provider.Should().Be(ViewModel.Provider);
            model.TlevelName.Should().Be(ViewModel.TlevelName);
            model.AcademicYear.Should().Be(ViewModel.AcademicYear);
            model.StartYear.Should().Be(ViewModel.StartYear);
            model.IndustryPlacementStatus.Should().Be(ViewModel.IndustryPlacementStatus);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(nameof(ViewModel.IndustryPlacementStatus));
            modelState[nameof(ViewModel.IndustryPlacementStatus)].Errors[0].ErrorMessage.Should().Be(AdminChangeIndustryPlacement.Validation_Message);
        }
    }
}
