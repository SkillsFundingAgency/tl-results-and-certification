using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using SpecialismQuestionContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SpecialismQuestion;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismQuestionPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            Controller.ModelState.AddModelError("HasLearnerDecidedSpecialism", SpecialismQuestionContent.Validation_Select_Yes_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeSpecialismQuestionViewModel));

            var model = viewResult.Model as ChangeSpecialismQuestionViewModel;
            model.Should().NotBeNull();
            
            Controller.ViewData.ModelState.ContainsKey(nameof(ChangeSpecialismQuestionViewModel.HasLearnerDecidedSpecialism)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ChangeSpecialismQuestionViewModel.HasLearnerDecidedSpecialism)];
            modelState.Errors[0].ErrorMessage.Should().Be(SpecialismQuestionContent.Validation_Select_Yes_Required_Message);
        }
    }
}
