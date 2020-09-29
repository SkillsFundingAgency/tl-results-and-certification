using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using UlnContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.UlnRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationUlnPost
{
    public class When_Called_With_Invalid_Uln : TestSetup
    {
        public override void Given()
        {
            UlnViewModel = new UlnViewModel { Uln = "HelloWorld" };
            Controller.ModelState.AddModelError("Uln", UlnContent.Validation_Uln_Must_Be_Digits);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
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
