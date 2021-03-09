using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using IndustryPlacementQuestionContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementQuestion;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddIndustryPlacementQuestionPost
{
    public class When_Model_Invalid : TestSetup
    {
        public override void Given()
        {
            
            IndustryPlacementQuestionViewModel = new IndustryPlacementQuestionViewModel();
            Controller.ModelState.AddModelError("IndustryPlacementStatus", IndustryPlacementQuestionContent.Validation_Select_Industry_Placement_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationErrorMessage()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IndustryPlacementQuestionViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(IndustryPlacementQuestionViewModel.IndustryPlacementStatus)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(IndustryPlacementQuestionViewModel.IndustryPlacementStatus)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(IndustryPlacementQuestionContent.Validation_Select_Industry_Placement_Required_Message);
        }
    }
}
