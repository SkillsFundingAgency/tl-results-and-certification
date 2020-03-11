using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncGet
{
    public class Then_Status_Not_AwaitingConfirmation_Redireced_To_PageNotFound : When_ReportIssueAsync_Is_Called
    {
        public override void Given()
        {

            expectedResult.PathwayStatusId = (int)TlevelReviewStatus.Confirmed;

            TlevelLoader.GetQueryTlevelViewModelAsync(ukprn, pathwayId)
                .Returns(expectedResult);
        }

        [Fact]
        public void Then_On_Status_Is_Not_AwaitingConfirmation_RedirectedTo_PageNotFound()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
