using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownGet
{
    public class When_Called_With_InValid_Data_For_Specialism : TestSetup
    {
        private PrsAddRommOutcomeKnownViewModel _addRommOutcomeKnownViewModel;

        public override void Given()
        {
            ProfileId = 0;
            AssessmentId = 7;
            ComponentType = ComponentType.Specialism;

            _addRommOutcomeKnownViewModel = null;

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addRommOutcomeKnownViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
