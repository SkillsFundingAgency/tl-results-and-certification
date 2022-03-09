using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownCoreGradeGet
{
    public class When_Called_With_InValid_Data : TestSetup
    {
        private PrsAddRommOutcomeKnownCoreGradeViewModel _addRommOutcomeKnownCoreGradeViewModel;

        public override void Given()
        {
            ProfileId = 0;
            AssessmentId = 7;

            _addRommOutcomeKnownCoreGradeViewModel = null;

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core).Returns(_addRommOutcomeKnownCoreGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
