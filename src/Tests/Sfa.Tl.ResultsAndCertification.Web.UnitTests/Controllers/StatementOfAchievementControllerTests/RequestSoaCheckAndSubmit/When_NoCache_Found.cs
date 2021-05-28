using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCheckAndSubmit
{
    public class When_NoCache_Found : TestSetup
    {
        private readonly FindSoaLearnerRecord _mockCache = null;

        public override void Given()
        {
            CacheService.GetAsync<FindSoaLearnerRecord>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            CacheService.Received(1).GetAsync<FindSoaLearnerRecord>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
