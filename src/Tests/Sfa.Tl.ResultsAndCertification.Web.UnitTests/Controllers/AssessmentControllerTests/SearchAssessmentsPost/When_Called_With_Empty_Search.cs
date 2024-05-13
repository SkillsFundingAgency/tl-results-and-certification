using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using SearchAssessmentsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.SearchAssessments;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.SearchAssessmentsPost
{
    public class When_Called_With_Empty_Search : TestSetup
    {
        public override void Given()
        {
            SearchAssessmentsViewModel = new SearchAssessmentsViewModel();
            Controller.ModelState.AddModelError("SearchUln", SearchAssessmentsContent.Uln_Required_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchAssessmentsViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SearchAssessmentsViewModel.SearchUln)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SearchAssessmentsViewModel.SearchUln)];
            modelState.Errors[0].ErrorMessage.Should().Be(SearchAssessmentsContent.Uln_Required_Validation_Message);
        }
    }
}
