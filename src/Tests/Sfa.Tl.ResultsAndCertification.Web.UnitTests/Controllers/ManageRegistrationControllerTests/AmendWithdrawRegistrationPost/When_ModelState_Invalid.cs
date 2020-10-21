using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using AmendWithdrawRegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.AmendWithdrawRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendWithdrawRegistrationPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AmendWithdrawRegistrationViewModel();
            Controller.ModelState.AddModelError(nameof(AmendWithdrawRegistrationViewModel.ChangeStatus), AmendWithdrawRegistrationContent.Select_Active_Options_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AmendWithdrawRegistrationViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(AmendWithdrawRegistrationViewModel.ChangeStatus)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(AmendActiveRegistrationViewModel.ChangeStatus)];
            modelState.Errors[0].ErrorMessage.Should().Be(AmendWithdrawRegistrationContent.Select_Active_Options_Validation_Message);
        }
    }
}
