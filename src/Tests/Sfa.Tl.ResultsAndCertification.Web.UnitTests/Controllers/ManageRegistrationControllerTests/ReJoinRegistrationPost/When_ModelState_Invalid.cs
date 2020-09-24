using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using RejoinRegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.RejoinRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.RejoinRegistrationPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new RejoinRegistrationViewModel();
            Controller.ModelState.AddModelError(nameof(RejoinRegistrationViewModel.CanRejoin), RejoinRegistrationContent.Select_Rejoin_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RejoinRegistrationViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(RejoinRegistrationViewModel.CanRejoin)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(RejoinRegistrationViewModel.CanRejoin)];
            modelState.Errors[0].ErrorMessage.Should().Be(RejoinRegistrationContent.Select_Rejoin_Validation_Message);
        }
    }
}
