using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using AmendActiveRegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.AmendActiveRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendActiveRegistrationPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AmendActiveRegistrationViewModel();
            Controller.ModelState.AddModelError(nameof(AmendActiveRegistrationViewModel.ChangeStatus), AmendActiveRegistrationContent.Select_Active_Options_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AmendActiveRegistrationViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(AmendActiveRegistrationViewModel.ChangeStatus)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(AmendActiveRegistrationViewModel.ChangeStatus)];
            modelState.Errors[0].ErrorMessage.Should().Be(AmendActiveRegistrationContent.Select_Active_Options_Validation_Message);
        }
    }
}
