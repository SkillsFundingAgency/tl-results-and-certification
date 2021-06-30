using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaNotAvailableNotResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaNotAvailableNoResults
{
    public class When_NoCache_Found : TestSetup
    {
        private readonly RequestSoaNotAvailableNoResultsViewModel _mockCache = null;

        public override void Given()
        {
            CacheService.GetAndRemoveAsync<RequestSoaNotAvailableNoResultsViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<RequestSoaNotAvailableNoResultsViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
