using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using UlnContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.UlnRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationUlnPost
{
    public class Then_On_Uln_Invalid_Returns_Validation_Error : When_AddRegistrationUln_Action_Is_Called
    {
        public override void Given()
        {
            UlnViewModel = new UlnViewModel { Uln = "HelloWorld" };
            Controller.ModelState.AddModelError("Uln", UlnContent.Validation_Uln_Must_Be_Digits);
        }

        [Fact]
        public void Then_Uln_Required_Validation_Message_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UlnViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(UlnViewModel.Uln)).Should().BeTrue();

            var modelStateUln = Controller.ViewData.ModelState[nameof(UlnViewModel.Uln)];
            modelStateUln.Errors[0].ErrorMessage.Should().Be(UlnContent.Validation_Uln_Must_Be_Digits);
        }
    }
}
