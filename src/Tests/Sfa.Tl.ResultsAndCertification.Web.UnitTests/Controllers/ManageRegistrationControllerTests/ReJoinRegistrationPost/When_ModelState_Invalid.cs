using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using ReJoinRegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.ReJoinRegistration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReJoinRegistrationPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ReJoinRegistrationViewModel();
            Controller.ModelState.AddModelError(nameof(ReJoinRegistrationViewModel.CanReJoin), ReJoinRegistrationContent.Select_ReJoin_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReJoinRegistrationViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ReJoinRegistrationViewModel.CanReJoin)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ReJoinRegistrationViewModel.CanReJoin)];
            modelState.Errors[0].ErrorMessage.Should().Be(ReJoinRegistrationContent.Select_ReJoin_Validation_Message);
        }
    }
}
