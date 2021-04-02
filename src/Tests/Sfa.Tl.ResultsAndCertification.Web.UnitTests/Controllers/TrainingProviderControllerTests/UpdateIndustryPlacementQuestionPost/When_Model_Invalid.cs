using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using IndustryPlacementQuestionContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementQuestion;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateIndustryPlacementQuestionPost
{
    public class When_Model_Invalid : TestSetup
    {
        public override void Given()
        {

            UpdateIndustryPlacementQuestionViewModel = new UpdateIndustryPlacementQuestionViewModel();
            Controller.ModelState.AddModelError("IndustryPlacementStatus", IndustryPlacementQuestionContent.Validation_Select_Industry_Placement_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationErrorMessage()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UpdateIndustryPlacementQuestionViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(UpdateIndustryPlacementQuestionViewModel.IndustryPlacementStatus)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(UpdateIndustryPlacementQuestionViewModel.IndustryPlacementStatus)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(IndustryPlacementQuestionContent.Validation_Select_Industry_Placement_Required_Message);
        }
    }
}