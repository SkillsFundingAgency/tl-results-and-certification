using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using EnglishAndMathsQuestionContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.EnglishAndMathsLrsQuestion;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishAndMathsLrsQuestionPost
{
    public class When_Model_Invalid : TestSetup
    {
        public override void Given()
        {
            EnglishAndMathsLrsQuestionViewModel = new EnglishAndMathsLrsQuestionViewModel();
            Controller.ModelState.AddModelError("EnglishAndMathsLrsStatus", EnglishAndMathsQuestionContent.Validation_Select_Is_EnglishMaths_Achieved_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationErrorMessage()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(EnglishAndMathsLrsQuestionViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(EnglishAndMathsLrsQuestionViewModel.EnglishAndMathsLrsStatus)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(EnglishAndMathsLrsQuestionViewModel.EnglishAndMathsLrsStatus)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(EnglishAndMathsQuestionContent.Validation_Select_Is_EnglishMaths_Achieved_Required_Message);
        }
    }
}
