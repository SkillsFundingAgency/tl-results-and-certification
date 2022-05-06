using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationHoursPost
{
    public class When_Called_With_Empty_Hours : TestSetup
    {
        public override void Given()
        {
            ViewModel = new SpecialConsiderationHoursViewModel
            {
                ProfileId = 1,
                LearnerName = "Test User",
                Hours = "0"
            };

            Controller.ModelState.AddModelError("Hours", Content.IndustryPlacement.IpSpecialConsiderationHours.Hours_Must_Be_Between_1_999);
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
            model.Hours.Should().Be(ViewModel.Hours);

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(SpecialConsiderationHoursViewModel.Hours)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SpecialConsiderationHoursViewModel.Hours)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpSpecialConsiderationHours.Hours_Must_Be_Between_1_999);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpCompletion);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(ViewModel.ProfileId.ToString());
        }
    }
}
