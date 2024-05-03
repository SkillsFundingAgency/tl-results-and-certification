using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using SearchResultsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.SearchResults;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.SearchResultsPost
{
    public class When_Called_With_Empty_Search : TestSetup
    {
        public override void Given()
        {
            SearchResultsViewModel = new SearchResultsViewModel();
            Controller.ModelState.AddModelError("SearchUln", SearchResultsContent.Uln_Required_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchResultsViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SearchResultsViewModel.SearchUln)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SearchResultsViewModel.SearchUln)];
            modelState.Errors[0].ErrorMessage.Should().Be(SearchResultsContent.Uln_Required_Validation_Message);
        }
    }
}
