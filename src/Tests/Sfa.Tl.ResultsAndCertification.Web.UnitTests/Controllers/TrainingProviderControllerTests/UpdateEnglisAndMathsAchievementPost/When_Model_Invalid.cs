using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using EnglishAndMathsQuestionContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.EnglishAndMathsQuestion;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateEnglisAndMathsAchievementPost
{
    public class When_Model_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new UpdateEnglishAndMathsQuestionViewModel();
            Controller.ModelState.AddModelError("EnglishAndMathsStatus", EnglishAndMathsQuestionContent.Validation_Select_Is_EnglishMaths_Achieved_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationErrorMessage()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UpdateEnglishAndMathsQuestionViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(UpdateEnglishAndMathsQuestionViewModel.EnglishAndMathsStatus)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(UpdateEnglishAndMathsQuestionViewModel.EnglishAndMathsStatus)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(EnglishAndMathsQuestionContent.Validation_Select_Is_EnglishMaths_Achieved_Required_Message);
        }
    }
}
