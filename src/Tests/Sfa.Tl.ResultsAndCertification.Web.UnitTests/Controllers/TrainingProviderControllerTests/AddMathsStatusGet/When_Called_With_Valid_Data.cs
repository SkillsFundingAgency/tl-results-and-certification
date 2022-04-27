using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddMathsStatusGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AddMathsStatusViewModel _addMathsStatusViewModel;

        public override void Given()
        {
            ProfileId = 1;

            _addMathsStatusViewModel = new AddMathsStatusViewModel { ProfileId = ProfileId, LearnerName = "Test Test", SubjectStatus = Common.Enum.SubjectStatus.NotSpecified };

            TrainingProviderLoader.GetLearnerRecordDetailsAsync<AddMathsStatusViewModel>(ProviderUkprn, ProfileId).Returns(_addMathsStatusViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<AddMathsStatusViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AddMathsStatusViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addMathsStatusViewModel.ProfileId);
            model.LearnerName.Should().Be(_addMathsStatusViewModel.LearnerName);
            model.IsAchieved.Should().BeNull();
            model.IsValid.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
