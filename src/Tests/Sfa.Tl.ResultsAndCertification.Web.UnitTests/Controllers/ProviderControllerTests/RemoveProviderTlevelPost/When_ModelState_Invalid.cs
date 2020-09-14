using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ProviderTlevelDetailsViewModel();
            Controller.ModelState.AddModelError("CanRemoveTlevel", Content.Provider.RemoveProviderTlevel.Select_RemoveProviderTlevel_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result.Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ProviderTlevelDetailsViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ProviderTlevelDetailsViewModel.CanRemoveTlevel)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ProviderTlevelDetailsViewModel.CanRemoveTlevel)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.Provider.RemoveProviderTlevel.Select_RemoveProviderTlevel_Validation_Message);
        }
    }
}
