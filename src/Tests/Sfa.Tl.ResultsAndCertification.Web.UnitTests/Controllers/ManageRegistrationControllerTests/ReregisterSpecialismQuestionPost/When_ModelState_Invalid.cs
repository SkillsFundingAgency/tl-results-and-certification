using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using SelectSpelismQuest = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SpecialismQuestion;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismQuestionPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            Controller.ModelState.AddModelError(nameof(ReregisterSpecialismQuestionViewModel.HasLearnerDecidedSpecialism), SelectSpelismQuest.Validation_Select_Yes_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterSpecialismQuestionViewModel));

            var model = viewResult.Model as ReregisterSpecialismQuestionViewModel;
            model.Should().NotBeNull();

            Controller.ViewData.ModelState.ContainsKey(nameof(ReregisterSpecialismQuestionViewModel.HasLearnerDecidedSpecialism)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ReregisterSpecialismQuestionViewModel.HasLearnerDecidedSpecialism)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectSpelismQuest.Validation_Select_Yes_Required_Message);
        }
    }
}
