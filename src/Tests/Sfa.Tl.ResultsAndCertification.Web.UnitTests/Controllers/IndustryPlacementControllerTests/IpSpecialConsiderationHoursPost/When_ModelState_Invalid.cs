using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationHoursPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new SpecialConsiderationHoursViewModel
            {
                ProfileId = 1,
                LearnerName = "Test User",
                Hours = null
            };

            Controller.ModelState.AddModelError("Hours", Content.IndustryPlacement.IpSpecialConsiderationHours.Hours_Required_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SpecialConsiderationHoursViewModel));

            var model = viewResult.Model as SpecialConsiderationHoursViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.Hours.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(SpecialConsiderationHoursViewModel.Hours)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SpecialConsiderationHoursViewModel.Hours)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpSpecialConsiderationHours.Hours_Required_Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpCompletion);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            // TODO Backloink
        }
    }
}
