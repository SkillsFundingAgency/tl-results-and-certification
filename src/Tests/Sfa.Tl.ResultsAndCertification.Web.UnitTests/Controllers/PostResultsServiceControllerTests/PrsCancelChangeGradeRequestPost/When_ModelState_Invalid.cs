using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelChangeGradeRequestPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsCancelGradeChangeRequestViewModel { ProfileId = 1, AssessmentId = 10, ComponentType = Common.Enum.ComponentType.Core, AreYouSureToCancel = null };
            Controller.ModelState.AddModelError("AreYouSureToCancel", Content.PostResultsService.PrsCancelGradeChangeRequest.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsCancelGradeChangeRequestViewModel));

            var model = viewResult.Model as PrsCancelGradeChangeRequestViewModel;
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsGradeChangeRequest);
            model.BackLink.RouteAttributes.Count.Should().Be(3);
            model.BackLink.RouteAttributes[Constants.ProfileId].Should().Be(ViewModel.ProfileId.ToString());
            model.BackLink.RouteAttributes[Constants.AssessmentId].Should().Be(ViewModel.AssessmentId.ToString());
            model.BackLink.RouteAttributes[Constants.ComponentType].Should().Be(((int)ViewModel.ComponentType).ToString());

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsCancelGradeChangeRequestViewModel.AreYouSureToCancel)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsCancelGradeChangeRequestViewModel.AreYouSureToCancel)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsCancelGradeChangeRequest.Validation_Message);
        }
    }
}
