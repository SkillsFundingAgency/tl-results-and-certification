using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelPost
{
    public class Then_On_ModelState_Invalid_Returns_Validation_Error : When_RemoveProviderTlevelAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            ViewModel = new ProviderTlevelDetailsViewModel();
            Controller.ModelState.AddModelError("CanRemoveTlevel", Content.Provider.RemoveProviderTlevel.Select_RemoveProviderTlevel_Validation_Message);
        }

        [Fact]
        public void Then_Expected_Error_Message_Is_Returned()
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
