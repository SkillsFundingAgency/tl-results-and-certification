using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncPost
{
    public class When_ReportIssue_Fail : TestSetup
    {
        public override void Given()
        {
            PathwayId = 99;
            InputViewModel = new TlevelQueryViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = PathwayId };
            TlevelLoader.ReportIssueAsync(InputViewModel).Returns(false);
        }


        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            TlevelLoader.Received(1).GetQueryTlevelViewModelAsync(AoUkprn, PathwayId);
        }

        [Fact]
        public void Then_Redirected_To_QueryServiceProblem()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.QueryServiceProblem);
        }
    }
}
