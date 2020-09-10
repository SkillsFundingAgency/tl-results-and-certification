using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using DateContent = Sfa.Tl.ResultsAndCertification.Web.Content.Helpers.Date;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeDateofBirthPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel.Day = string.Empty;
            Controller.ModelState.AddModelError("Day", DateContent.Validation_Message_Day_Required);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeDateofBirthViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ChangeDateofBirthViewModel.Day)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ChangeDateofBirthViewModel.Day)];
            modelState.Errors[0].ErrorMessage.Should().Be(DateContent.Validation_Message_Day_Required);
        }
    }
}
