using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddWithdrawnStatusGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private AddWithdrawnStatusViewModel _addWithdrawnStatusViewModel;

        public override void Given()
        {
            ProfileId = 0;

            _addWithdrawnStatusViewModel = null;

            TrainingProviderLoader.GetLearnerRecordDetailsAsync<AddWithdrawnStatusViewModel>(ProviderUkprn, ProfileId).Returns(_addWithdrawnStatusViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<AddWithdrawnStatusViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
