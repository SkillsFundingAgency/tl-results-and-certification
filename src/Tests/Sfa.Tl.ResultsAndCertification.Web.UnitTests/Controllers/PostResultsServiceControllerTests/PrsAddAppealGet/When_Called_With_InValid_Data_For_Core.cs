using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradeGet
{
    public class When_Called_With_InValid_Data_For_Core : TestSetup
    {
        private PrsAddAppealViewModel _appealCoreGradeViewModel;

        public override void Given()
        {
            ProfileId = 0;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;
            _appealCoreGradeViewModel = null;

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<ComponentType>()).Returns(_appealCoreGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<ComponentType>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
