using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddMathsStatusGet
{
    public class When_Called_With_Invalid_Subject_Status : TestSetup
    {
        private AddMathsStatusViewModel _addMathsStatusViewModel;

        public override void Given()
        {
            ProfileId = 1;

            _addMathsStatusViewModel = new AddMathsStatusViewModel { ProfileId = ProfileId, LearnerName = "Test Test", SubjectStatus = Common.Enum.SubjectStatus.Achieved };

            TrainingProviderLoader.GetLearnerRecordDetailsAsync<AddMathsStatusViewModel>(ProviderUkprn, ProfileId).Returns(_addMathsStatusViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<AddMathsStatusViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
