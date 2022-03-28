using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeChangeGet
{
    public class When_Called_With_InValid_Data_For_Core : TestSetup
    {
        private PrsAppealGradeChangeViewModel _appealGradeChangeViewModel;

        public override void Given()
        {
            ProfileId = 0;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _appealGradeChangeViewModel = null;

            Loader.GetPrsLearnerDetailsAsync<PrsAppealGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_appealGradeChangeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAppealGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
