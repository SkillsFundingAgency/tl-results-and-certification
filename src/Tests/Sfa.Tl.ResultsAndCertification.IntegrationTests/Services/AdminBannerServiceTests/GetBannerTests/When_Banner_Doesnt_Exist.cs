using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminBannerServiceTests.GetBannerTests
{
    public class When_Banner_Doesnt_Exist : GetBannerBaseTest
    {
        private const int NonExistentBannerId = 999;

        public override void Given()
        {
            BannerId = NonExistentBannerId;
            SeedTestData();
        }

        [Fact]
        public void Then_Should_Return_Null()
        {
            Result.Should().BeNull();
        }
    }
}