using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;
using SelectSpecialismContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectSpecialisms;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismsPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ReregisterSpecialismViewModel();
            Controller.ModelState.AddModelError(nameof(ReregisterSpecialismViewModel.HasSpecialismSelected), SelectSpecialismContent.Validation_Select_Specialism_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterSpecialismViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(ReregisterSpecialismViewModel.HasSpecialismSelected)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ReregisterSpecialismViewModel.HasSpecialismSelected)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectSpecialismContent.Validation_Select_Specialism_Required_Message);
        }
    }
}
