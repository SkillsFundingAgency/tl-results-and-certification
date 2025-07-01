using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeLevelTwoMathsPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private const int ExpectedRegistrationPathwayId = 1;

        public override void Given()
        {
            ViewModel = CreateViewModel(ExpectedRegistrationPathwayId, SubjectStatus.NotAchieved, null);

            Controller.ModelState.AddModelError(nameof(ViewModel.MathsStatusTo), AdminChangeLevelTwoMaths.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = Result.ShouldBeViewResult<AdminChangeMathsResultsViewModel>();

            model.Should().NotBeNull();
            model.RegistrationPathwayId.Should().Be(ViewModel.RegistrationPathwayId);
            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.Uln.Should().Be(ViewModel.Uln);
            model.Provider.Should().Be(ViewModel.Provider);
            model.TlevelName.Should().Be(ViewModel.TlevelName);
            model.AcademicYear.Should().Be(ViewModel.AcademicYear);
            model.StartYear.Should().Be(ViewModel.StartYear);
            model.MathsStatus.Should().Be(ViewModel.MathsStatus);
            model.MathsStatusTo.Should().Be(ViewModel.MathsStatusTo);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(nameof(ViewModel.MathsStatusTo));
            modelState[nameof(ViewModel.MathsStatusTo)].Errors[0].ErrorMessage.Should().Be(AdminChangeLevelTwoMaths.Validation_Message);
        }
    }
}