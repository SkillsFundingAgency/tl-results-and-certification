using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using PrsSearchLearnerContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsSearchLearner;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSearchLearnerPost
{
    public class When_Uln_IsEmpty : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsSearchLearnerViewModel { SearchUln = string.Empty };
            Controller.ModelState.AddModelError("SearchUln", PrsSearchLearnerContent.Uln_Required_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsSearchLearnerViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(PrsSearchLearnerViewModel.SearchUln)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(PrsSearchLearnerViewModel.SearchUln)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(PrsSearchLearnerContent.Uln_Required_Validation_Message);
        }
    }
}