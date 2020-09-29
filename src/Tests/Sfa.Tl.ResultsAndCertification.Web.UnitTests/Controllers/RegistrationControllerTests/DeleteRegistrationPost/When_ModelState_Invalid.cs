using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DeleteRegistrationPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            Controller.ModelState.AddModelError("DeleteRegistration", Content.Registration.DeleteRegistration.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(DeleteRegistrationViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(DeleteRegistrationViewModel.DeleteRegistration)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(DeleteRegistrationViewModel.DeleteRegistration)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.Registration.DeleteRegistration.Validation_Message);
        }
    }
}
