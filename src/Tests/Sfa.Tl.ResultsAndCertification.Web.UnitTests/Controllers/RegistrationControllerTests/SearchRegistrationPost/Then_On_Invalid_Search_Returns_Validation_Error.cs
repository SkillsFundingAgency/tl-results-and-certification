using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using SearchRegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SearchRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SearchRegistrationPost
{
    public class Then_On_Invalid_Search_Returns_Validation_Error : When_SearchRegistration_Post_Action_Is_Called
    {
        private string _searchUln = "12345678";
        public override void Given()
        {
            SearchRegistrationViewModel = new SearchRegistrationViewModel { SearchUln = _searchUln };
            Controller.ModelState.AddModelError("SearchUln", SearchRegistrationContent.Uln_Not_Valid_Validation_Message);
        }

        [Fact]
        public void Then_Expected_Required_Validation_Error_Are_Returned()
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
