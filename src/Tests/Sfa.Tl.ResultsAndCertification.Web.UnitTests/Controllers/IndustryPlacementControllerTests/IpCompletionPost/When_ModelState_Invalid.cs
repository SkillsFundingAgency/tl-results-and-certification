using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private IpCompletionViewModel _ipCompletionViewModel;

        public override void Given()
        {
            ProfileId = 1;

            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = ProfileId,
                PathwayId = 1,
                AcademicYear = 2020,
                LearnerName = "John Smith",
                IndustryPlacementStatus = null
            };


            ViewModel = new IpCompletionViewModel { ProfileId = ProfileId, PathwayId = 1, AcademicYear = 2020, LearnerName = "John Smith", IndustryPlacementStatus = null };
            Controller.ModelState.AddModelError("IndustryPlacementStatus", Content.IndustryPlacement.IpCompletion.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpCompletionViewModel));

            var model = viewResult.Model as IpCompletionViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_ipCompletionViewModel.ProfileId);
            model.PathwayId.Should().Be(_ipCompletionViewModel.PathwayId);
            model.LearnerName.Should().Be(_ipCompletionViewModel.LearnerName);
            model.AcademicYear.Should().Be(_ipCompletionViewModel.AcademicYear);
            model.IndustryPlacementStatus.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(IpCompletionViewModel.IndustryPlacementStatus)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(IpCompletionViewModel.IndustryPlacementStatus)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpCompletion.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
