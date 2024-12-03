using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminBannerServiceTests.GetBannerTests
{
    public class When_Banner_Exist : GetBannerBaseTest
    {
        private const int ExistingBannerId = 1;

        public override void Given()
        {
            BannerId = ExistingBannerId;
            SeedTestData();
        }

        [Fact]
        public void Then_Should_Return_Expected()
        {
            Result.Should().NotBeNull();

            Result.Id.Should().Be(ExistingBannerId);
            Result.Title.Should().Be(FirstBanner.Title);
            Result.Content.Should().Be(FirstBanner.Content);
            Result.Target.Should().Be(FirstBanner.Target);
            Result.Start.Should().Be(FirstBanner.Start);
            Result.End.Should().Be(FirstBanner.End);
            Result.IsOptedin.Should().Be(FirstBanner.IsOptedin);
        }
    }
}