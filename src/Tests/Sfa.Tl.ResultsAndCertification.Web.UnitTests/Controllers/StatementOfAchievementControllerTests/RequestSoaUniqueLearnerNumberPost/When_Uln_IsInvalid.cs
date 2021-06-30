using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;
using RequestSoaUniqueLearnerNumberContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaUniqueLearnerNumber;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaUniqueLearnerNumberPost
{
    public class When_Uln_IsInvalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new RequestSoaUniqueLearnerNumberViewModel { SearchUln = "xyz" };
            Controller.ModelState.AddModelError("SearchUln", RequestSoaUniqueLearnerNumberContent.Uln_Not_Valid_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationErrorMessage()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RequestSoaUniqueLearnerNumberViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(RequestSoaUniqueLearnerNumberViewModel.SearchUln)).Should().BeTrue();
            var modelState = Controller.ViewData.ModelState[nameof(RequestSoaUniqueLearnerNumberViewModel.SearchUln)];
            modelState.Errors.Count.Should().Be(1);
            modelState.Errors[0].ErrorMessage.Should().Be(RequestSoaUniqueLearnerNumberContent.Uln_Not_Valid_Validation_Message);
        }
    }
}
