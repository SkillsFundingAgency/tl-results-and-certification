using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.AddCoreResultPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel.SelectedGradeCode = string.Empty;

            ResultLoader.GetManageCoreResultAsync(AoUkprn, ProfileId, 1, false).Returns(new ManageCoreResultViewModel());
            Controller.ModelState.AddModelError("SelectedGradeCode", Content.Result.ManageCoreResult.Validation_Select_Grade_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ManageCoreResultViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ManageCoreResultViewModel.SelectedGradeCode)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ManageCoreResultViewModel.SelectedGradeCode)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.Result.ManageCoreResult.Validation_Select_Grade_Required_Message);
        }
    }
}
