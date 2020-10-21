using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeLearnersNamePost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            Controller.ModelState.AddModelError("Firstname", Content.Registration.LearnersName.Validation_Firstname_Required);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeLearnersNameViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ChangeLearnersNameViewModel.Firstname)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ChangeLearnersNameViewModel.Firstname)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.Registration.LearnersName.Validation_Firstname_Required);
        }
    }
}
