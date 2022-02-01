using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeSpecialismResultPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel.SelectedGradeCode = null;

            ResultLoader.GetManageSpecialismResultAsync(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, true).Returns(new ManageSpecialismResultViewModel());
            Controller.ModelState.AddModelError("SelectedGradeCode", Content.Result.ManageSpecialismResult.Validation_Select_Grade_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ManageSpecialismResultViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ManageSpecialismResultViewModel.SelectedGradeCode)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ManageSpecialismResultViewModel.SelectedGradeCode)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.Result.ManageSpecialismResult.Validation_Select_Grade_Required_Message);
        }
    }
}
