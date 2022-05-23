using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ProfileId = 1;
            ViewModel = new IpModelUsedViewModel { ProfileId = ProfileId, LearnerName = "John Smith", IsIpModelUsed = null };
            Controller.ModelState.AddModelError("IsIpModelUsed", Content.IndustryPlacement.IpModelUsed.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpModelUsedViewModel));

            var model = viewResult.Model as IpModelUsedViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.IsIpModelUsed.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(IpModelUsedViewModel.IsIpModelUsed)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(IpModelUsedViewModel.IsIpModelUsed)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpModelUsed.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpCompletion);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
