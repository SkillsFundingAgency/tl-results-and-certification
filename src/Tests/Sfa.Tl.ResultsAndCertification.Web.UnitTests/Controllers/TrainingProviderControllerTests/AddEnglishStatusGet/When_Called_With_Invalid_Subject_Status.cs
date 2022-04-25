using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishStatusGet
{
    public class When_Called_With_Invalid_Subject_Status : TestSetup
    {
        private AddEnglishStatusViewModel _addEnglishStatusViewModel;

        public override void Given()
        {
            ProfileId = 1;

            _addEnglishStatusViewModel = new AddEnglishStatusViewModel { ProfileId = ProfileId, LearnerName = "Test Test", SubjectStatus = Common.Enum.SubjectStatus.Achieved };

            TrainingProviderLoader.GetLearnerRecordDetailsAsync<AddEnglishStatusViewModel>(ProviderUkprn, ProfileId).Returns(_addEnglishStatusViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<AddEnglishStatusViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
