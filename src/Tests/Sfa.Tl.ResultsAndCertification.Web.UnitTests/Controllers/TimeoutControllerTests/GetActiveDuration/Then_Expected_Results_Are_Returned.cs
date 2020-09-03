using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TimeoutControllerTests.GetActiveDuration
{
    public class Then_Expected_Results_Are_Returned : When_GetActiveDurationAsync_Action_Is_Called
    {
        private DateTime _cacheResult;
        public override void Given()
        {
            _cacheResult = DateTime.UtcNow;
            CacheService.GetAsync<DateTime>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Data_Is_Returned()
        {
            Result.Result.Should().NotBeNull();
            Result.Result.Should().BeOfType(typeof(JsonResult));

            var actualResult = Result.Result.Value as SessionActivityData;

            actualResult.Minutes.Should().NotBe(0);
            actualResult.Seconds.Should().NotBe(0);
        }
    }
}
