using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using SearchRegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SearchRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SearchRegistrationPost
{
    public class Then_On_Empty_Search_Returns_Required_Validation_Error : When_SearchRegistration_Post_Action_Is_Called
    {        
        public override void Given()
        {
            SearchRegistrationViewModel = new SearchRegistrationViewModel();
            Controller.ModelState.AddModelError("SearchUln", SearchRegistrationContent.Uln_Required_Validation_Message);
        }

        [Fact]
        public void Then_Expected_Required_Validation_Error_Are_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SearchRegistrationViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SearchRegistrationViewModel.SearchUln)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SearchRegistrationViewModel.SearchUln)];
            modelState.Errors[0].ErrorMessage.Should().Be(SearchRegistrationContent.Uln_Required_Validation_Message);
        }
    }
}
