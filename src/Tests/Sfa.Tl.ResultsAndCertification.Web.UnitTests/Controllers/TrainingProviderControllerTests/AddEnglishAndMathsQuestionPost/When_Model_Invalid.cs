using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using EnglishAndMathsQuestionContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.EnglishAndMathsQuestion;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishAndMathsQuestionPost
{
    public class When_Model_Invalid : TestSetup
    {
        public override void Given()
        {
            EnglishAndMathsQuestionViewModel = new EnglishAndMathsQuestionViewModel();
            Controller.ModelState.AddModelError("EnglishAndMathsStatus", EnglishAndMathsQuestionContent.Validation_Select_Is_EnglishMaths_Achieved_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationErrorMessage()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(EnglishAndMathsQuestionViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(EnglishAndMathsQuestionViewModel.EnglishAndMathsStatus)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(EnglishAndMathsQuestionViewModel.EnglishAndMathsStatus)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(EnglishAndMathsQuestionContent.Validation_Select_Is_EnglishMaths_Achieved_Required_Message);
        }
    }
}
