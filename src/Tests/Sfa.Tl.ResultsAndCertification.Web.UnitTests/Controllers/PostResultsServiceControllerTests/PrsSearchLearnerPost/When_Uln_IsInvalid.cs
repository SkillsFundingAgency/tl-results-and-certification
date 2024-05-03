using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using PrsSearchLearnerContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsSearchLearner;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSearchLearnerPost
{
    public class When_Uln_IsInvalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsSearchLearnerViewModel { SearchUln = "xyz" };
            Controller.ModelState.AddModelError("SearchUln", PrsSearchLearnerContent.Uln_Not_Valid_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationErrorMessage()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsSearchLearnerViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(PrsSearchLearnerViewModel.SearchUln)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(PrsSearchLearnerViewModel.SearchUln)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(PrsSearchLearnerContent.Uln_Not_Valid_Validation_Message);
        }
    }
}
