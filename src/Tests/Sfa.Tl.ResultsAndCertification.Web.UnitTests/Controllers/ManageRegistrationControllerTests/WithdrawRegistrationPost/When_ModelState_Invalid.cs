using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using WithdrawRegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.WithdrawRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawRegistrationPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new WithdrawRegistrationViewModel();
            Controller.ModelState.AddModelError(nameof(WithdrawRegistrationViewModel.CanWithdraw), WithdrawRegistrationContent.Select_Withdraw_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(WithdrawRegistrationViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(WithdrawRegistrationViewModel.CanWithdraw)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(WithdrawRegistrationViewModel.CanWithdraw)];
            modelState.Errors[0].ErrorMessage.Should().Be(WithdrawRegistrationContent.Select_Withdraw_Validation_Message);
        }
    }
}
