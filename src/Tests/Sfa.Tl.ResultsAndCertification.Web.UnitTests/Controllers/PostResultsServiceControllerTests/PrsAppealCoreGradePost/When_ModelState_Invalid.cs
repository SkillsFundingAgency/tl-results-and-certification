using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradePost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AppealCoreGradeViewModel { ProfileId = 1, AppealGrade = null };
            Controller.ModelState.AddModelError("AppealGrade", Content.PostResultsService.AppealCoreGrade.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AppealCoreGradeViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(AppealCoreGradeViewModel.AppealGrade)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(AppealCoreGradeViewModel.AppealGrade)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.AppealCoreGrade.Validation_Message);
        }
    }
}
