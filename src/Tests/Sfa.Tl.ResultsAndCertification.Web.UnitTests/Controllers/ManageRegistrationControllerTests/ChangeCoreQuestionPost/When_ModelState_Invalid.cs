using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using ChangeCoreQuestionContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.ChangeCoreQuestion;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeCoreQuestionPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ChangeCoreQuestionViewModel();
            Controller.ModelState.AddModelError("CanChangeCore", ChangeCoreQuestionContent.Select_ChangeCoreQuestion_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeCoreQuestionViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ChangeCoreQuestionViewModel.CanChangeCore)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ChangeCoreQuestionViewModel.CanChangeCore)];
            modelState.Errors[0].ErrorMessage.Should().Be(ChangeCoreQuestionContent.Select_ChangeCoreQuestion_Validation_Message);
        }
    }
}
