using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using SearchPostResultsServiceContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.SearchPostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.SearchPostResultsServicePost
{
    public class When_Uln_IsInvalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new SearchPostResultsServiceViewModel { SearchUln = "xyz" };
            Controller.ModelState.AddModelError("SearchUln", SearchPostResultsServiceContent.Uln_Not_Valid_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationErrorMessage()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchPostResultsServiceViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SearchPostResultsServiceViewModel.SearchUln)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(SearchPostResultsServiceViewModel.SearchUln)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(SearchPostResultsServiceContent.Uln_Not_Valid_Validation_Message);
        }
    }
}
