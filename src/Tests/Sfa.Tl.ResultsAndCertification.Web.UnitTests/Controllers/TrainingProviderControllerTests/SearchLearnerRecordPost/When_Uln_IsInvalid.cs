using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using SearchUlnContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SearchLearnerRecord;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordPost
{
    public class When_Uln_IsInvalid : TestSetup
    {
        public override void Given()
        {
            var uln = "Invalid123";

            SearchLearnerRecordViewModel = new SearchLearnerRecordViewModel { SearchUln = uln };
            Controller.ModelState.AddModelError("SearchUln", SearchUlnContent.Uln_Not_Valid_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationErrorMessage()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchLearnerRecordViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SearchLearnerRecordViewModel.SearchUln)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(SearchLearnerRecordViewModel.SearchUln)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(SearchUlnContent.Uln_Not_Valid_Validation_Message);
        }
    }
}