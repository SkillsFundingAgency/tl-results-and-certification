using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.CancelRegistrationPost
{
    public class Then_On_ModelState_Invalid_Returns_Validation_Error : When_CancelRegistrationAsync_Is_Called
    {
        public override void Given()
        {
            Controller.ModelState.AddModelError("CancelRegistration", Content.Registration.CancelRegistration.Validation_Message);
        }

        [Fact]
        public void Then_Expected_Error_Message_Is_Returned()
        {
           Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(CancelRegistrationViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(CancelRegistrationViewModel.CancelRegistration)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(CancelRegistrationViewModel.CancelRegistration)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.Registration.CancelRegistration.Validation_Message);
        }
    }
}
