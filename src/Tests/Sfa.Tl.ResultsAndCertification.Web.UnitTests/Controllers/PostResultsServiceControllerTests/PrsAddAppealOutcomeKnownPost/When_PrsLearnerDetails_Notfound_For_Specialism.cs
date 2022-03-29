using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomeKnownPost
{
    public class When_PrsLearnerDetails_Notfound_For_Specialism : TestSetup
    {
        private readonly PrsAddAppealOutcomeKnownViewModel _mockLoaderResponse = null;

        public override void Given()
        {
            ComponentType = Common.Enum.ComponentType.Specialism;
            ViewModel = new PrsAddAppealOutcomeKnownViewModel { ProfileId = 0, AssessmentId = 11, ComponentType = ComponentType };
            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType)
                  .Returns(_mockLoaderResponse);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
