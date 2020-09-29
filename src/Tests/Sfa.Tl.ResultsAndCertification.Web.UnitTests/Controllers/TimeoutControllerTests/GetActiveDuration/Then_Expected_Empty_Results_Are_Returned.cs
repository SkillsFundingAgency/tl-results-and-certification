using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TimeoutControllerTests.GetActiveDuration
{
    public class Then_Expected_Empty_Results_Are_Returned : When_GetActiveDurationAsync_Action_Is_Called
    {
        public override void Given()
        {
            var cacheResult = DateTime.MinValue;
            CacheService.GetAsync<DateTime>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_Empty_Data_Is_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(JsonResult));

            var actualResult = Result.Value as SessionActivityData;

            actualResult.Minutes.Should().Be(0);
            actualResult.Seconds.Should().Be(0);
        }
    }
}
