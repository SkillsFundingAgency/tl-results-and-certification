using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpEmployerLedUsedPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new IpEmployerLedUsedViewModel { LearnerName = "John Smith", IsEmployerLedUsed = null };
            Controller.ModelState.AddModelError("IsEmployerLedUsed", Content.IndustryPlacement.IpEmployerLedUsed.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpEmployerLedUsedViewModel));

            var model = viewResult.Model as IpEmployerLedUsedViewModel;

            model.Should().NotBeNull();

            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.IsEmployerLedUsed.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(IpEmployerLedUsedViewModel.IsEmployerLedUsed)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(IpEmployerLedUsedViewModel.IsEmployerLedUsed)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpEmployerLedUsed.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpBlendedPlacementUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
