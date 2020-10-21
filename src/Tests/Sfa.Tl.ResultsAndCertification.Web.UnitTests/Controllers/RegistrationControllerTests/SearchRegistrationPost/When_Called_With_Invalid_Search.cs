using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using SearchRegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SearchRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SearchRegistrationPost
{
    public class When_Called_With_Invalid_Search : TestSetup
    {
        private string _searchUln = "12345678";
        public override void Given()
        {
            SearchRegistrationViewModel = new SearchRegistrationViewModel { SearchUln = _searchUln };
            Controller.ModelState.AddModelError("SearchUln", SearchRegistrationContent.Uln_Not_Valid_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchRegistrationViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SearchRegistrationViewModel.SearchUln)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SearchRegistrationViewModel.SearchUln)];
            modelState.Errors[0].ErrorMessage.Should().Be(SearchRegistrationContent.Uln_Not_Valid_Validation_Message);
        }
    }
}
