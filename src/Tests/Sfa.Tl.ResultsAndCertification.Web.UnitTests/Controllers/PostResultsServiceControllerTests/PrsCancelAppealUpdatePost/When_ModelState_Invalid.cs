using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelAppealUpdatePost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsCancelAppealUpdateViewModel { ProfileId = 1, AreYouSureToCancel = null };
            Controller.ModelState.AddModelError("AreYouSureToCancel", Content.PostResultsService.PrsCancelAppealUpdate.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsCancelAppealUpdateViewModel));

            var model = viewResult.Model as PrsCancelAppealUpdateViewModel;
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAppealCheckAndSubmit);
            model.BackLink.RouteAttributes.Count.Should().Be(0);

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsCancelAppealUpdateViewModel.AreYouSureToCancel)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsCancelAppealUpdateViewModel.AreYouSureToCancel)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsCancelAppealUpdate.Validation_Message);
        }
    }
}
