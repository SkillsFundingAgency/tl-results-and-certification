using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsWithdrawAppealTests
{
    public class When_WithdrawAppeal_IsNotSuccess : TestSetup
    {
        public override void Given()
        {
            var isAppealSuccess = false;
            Loader.WithdrawAppealCoreGradeAsync(AoUkprn, ViewModel).Returns(isAppealSuccess);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            Loader.Received(1).WithdrawAppealCoreGradeAsync(AoUkprn, Arg.Any<AppealOutcomePathwayGradeViewModel>());
        }
    }
}
