using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncPost
{
    public class When_ViewModel_IsValid : TestSetup
    {
        public override void Given()
        {
            PathwayId = 99;
            InputViewModel = new TlevelQueryViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = PathwayId };
            TlevelLoader.GetQueryTlevelViewModelAsync(AoUkprn, PathwayId).Returns(ExpectedResult);
            TlevelLoader.ReportIssueAsync(InputViewModel).Returns(true);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            TlevelLoader.Received(1).GetQueryTlevelViewModelAsync(AoUkprn, PathwayId);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<TlevelQuerySentViewModel>(x => x.TlevelTitle == ExpectedResult.TlevelTitle));
        }

        [Fact]
        public void Then_Redirected_To_QueryTlevelSent()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.QueryTlevelSent);
        }
    }
}
