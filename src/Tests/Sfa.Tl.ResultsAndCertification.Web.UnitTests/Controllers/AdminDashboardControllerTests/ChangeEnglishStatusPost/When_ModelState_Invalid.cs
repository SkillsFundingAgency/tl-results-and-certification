using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeEnglishStatusPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private const int ExpectedRegistrationPathwayId = 1;

        public override void Given()
        {
            ViewModel = CreateViewModel(ExpectedRegistrationPathwayId, SubjectStatus.NotAchieved, null);

            Controller.ModelState.AddModelError(nameof(ViewModel.EnglishStatusTo), AdminChangeEnglishStatus.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = Result.ShouldBeViewResult<AdminChangeEnglishResultsViewModel>();

            model.Should().NotBeNull();
            model.RegistrationPathwayId.Should().Be(ViewModel.RegistrationPathwayId);
            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.Uln.Should().Be(ViewModel.Uln);
            model.Provider.Should().Be(ViewModel.Provider);
            model.TlevelName.Should().Be(ViewModel.TlevelName);
            model.AcademicYear.Should().Be(ViewModel.AcademicYear);
            model.StartYear.Should().Be(ViewModel.StartYear);
            model.EnglishStatus.Should().Be(ViewModel.EnglishStatus);
            model.EnglishStatusTo.Should().Be(ViewModel.EnglishStatusTo);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(nameof(ViewModel.EnglishStatusTo));
            modelState[nameof(ViewModel.EnglishStatusTo)].Errors[0].ErrorMessage.Should().Be(AdminChangeEnglishStatus.Validation_Message);
        }
    }
}