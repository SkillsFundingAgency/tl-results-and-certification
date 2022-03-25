using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradeGet
{
    public class When_Called_With_Invalid_PrsStatus_For_Core : TestSetup
    {
        private PrsAddAppealViewModel _addAppealViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _addAppealViewModel = new PrsAddAppealViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,                
                PrsStatus = PrsStatus.UnderReview,
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addAppealViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
