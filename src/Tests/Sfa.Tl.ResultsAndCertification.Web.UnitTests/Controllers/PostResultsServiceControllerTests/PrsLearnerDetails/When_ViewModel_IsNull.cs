using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsLearnerDetails
{
    public class When_ViewModel_IsNull : TestSetup
    {
        private readonly PrsLearnerDetailsViewModel1 _mockLearnerDetails = null;

        public override void Given()
        {
            ProfileId = 11;

            Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel1>(AoUkprn, ProfileId).Returns(_mockLearnerDetails);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel1>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
