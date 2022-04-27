using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishStatusGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AddEnglishStatusViewModel _addEnglishStatusViewModel;

        public override void Given()
        {
            ProfileId = 1;

            _addEnglishStatusViewModel = new AddEnglishStatusViewModel { ProfileId = ProfileId, LearnerName = "Test Test", SubjectStatus = Common.Enum.SubjectStatus.NotSpecified };

            TrainingProviderLoader.GetLearnerRecordDetailsAsync<AddEnglishStatusViewModel>(ProviderUkprn, ProfileId).Returns(_addEnglishStatusViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<AddEnglishStatusViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AddEnglishStatusViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_addEnglishStatusViewModel.ProfileId);
            model.LearnerName.Should().Be(_addEnglishStatusViewModel.LearnerName);
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
