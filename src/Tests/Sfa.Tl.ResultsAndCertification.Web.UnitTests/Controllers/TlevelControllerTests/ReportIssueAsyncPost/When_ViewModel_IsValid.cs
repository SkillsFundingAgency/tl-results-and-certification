using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncPost
{
    public class When_ViewModel_IsValid : TestSetup
    {
        public override void Given()
        {
            PathwayId = 99;
            InputViewModel = new TlevelQueryViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = PathwayId };
            TlevelLoader.ReportIssueAsync(InputViewModel).Returns(true);
        }

        [Fact]
        public void Then_Redirected_To_TlevelDetailsQueriedConfirmation()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.TlevelDetailsQueriedConfirmation);
        }
    }
}
