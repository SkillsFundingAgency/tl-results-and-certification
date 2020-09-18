using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TimeoutControllerTests.RenewSessionActivity
{
    public class Then_Expected_Results_Are_Returned : When_RenewSessionActivityAsync_Action_Is_Called
    {
        public override void Given() { }

        [Fact]
        public void Then_RenewSessionActivity_Cache_Is_Synchronised()
        {
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<DateTime>());
        }

        [Fact]
        public void Then_Expected_Data_Is_Returned()
        {
            Result.Result.Should().NotBeNull();
            Result.Result.Should().BeOfType(typeof(JsonResult));

            var actualResult = Result.Result.Value as SessionActivityData;

            actualResult.Minutes.Should().Be(_resultsAndCertificationConfiguration.DfeSignInSettings.Timeout);
            actualResult.Seconds.Should().Be(0);
        }
    }
}
