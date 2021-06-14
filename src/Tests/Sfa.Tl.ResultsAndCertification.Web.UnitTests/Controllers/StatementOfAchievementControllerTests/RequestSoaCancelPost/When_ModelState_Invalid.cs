using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCancelPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new RequestSoaCancelViewModel { ProfileId = 1, CancelRequest = null };
            Controller.ModelState.AddModelError("CancelRequest", Content.StatementOfAchievement.RequestSoaCancel.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RequestSoaCancelViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(RequestSoaCancelViewModel.CancelRequest)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(RequestSoaCancelViewModel.CancelRequest)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.StatementOfAchievement.RequestSoaCancel.Validation_Message);
        }
    }
}
