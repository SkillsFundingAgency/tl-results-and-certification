using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddMathsStatusPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AddMathsStatusViewModel _addMathsStatusViewModel;

        public override void Given()
        {
            ProfileId = 1;

            _addMathsStatusViewModel = new AddMathsStatusViewModel
            {
                ProfileId = ProfileId,
                LearnerName = "John Smith",
                IsAchieved = null
            };


            ViewModel = new AddMathsStatusViewModel { ProfileId = 1, LearnerName = "John Smith", IsAchieved = null };
            Controller.ModelState.AddModelError("IsAchieved", Content.TrainingProvider.AddMathsStatus.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AddMathsStatusViewModel));

            var model = viewResult.Model as AddMathsStatusViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_addMathsStatusViewModel.ProfileId);
            model.LearnerName.Should().Be(_addMathsStatusViewModel.LearnerName);
            model.IsAchieved.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(AddMathsStatusViewModel.IsAchieved)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(AddMathsStatusViewModel.IsAchieved)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.TrainingProvider.AddMathsStatus.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
