using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using SelectSpecialismContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectSpecialisms;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            SelectSpecialismViewModel = new SelectSpecialismViewModel();
            Controller.ModelState.AddModelError("HasSpecialismSelected", SelectSpecialismContent.Validation_Select_Specialism_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectSpecialismViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SelectSpecialismViewModel.HasSpecialismSelected)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SelectSpecialismViewModel.HasSpecialismSelected)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectSpecialismContent.Validation_Select_Specialism_Required_Message);
        }
    }
}
