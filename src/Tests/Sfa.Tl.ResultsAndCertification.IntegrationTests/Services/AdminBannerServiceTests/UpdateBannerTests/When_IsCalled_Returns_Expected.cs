using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using System;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminBannerServiceTests.UpdateBannerTests
{
    public class When_IsCalled_Returns_Expected : UpdateBannerBaseTest
    {
        private const int BannerId = 1;

        public override void Given()
        {
            Request = new UpdateBannerRequest
            {
                BannerId = BannerId,
                Title = "updated-banner-title",
                Content = "updated-banner-content",
                Target = BannerTarget.Provider,
                Start = new DateTime(2025, 1, 15),
                End = new DateTime(2025, 1, 27),
                IsActive = true,
                ModifiedBy = "test-user"
            };

            SeedTestData();
        }

        [Fact]
        public void Then_Should_Return_True()
        {
            Result.Should().BeTrue();

            Banner bannerDb = DbContext.Banner.Single(b => b.Id == Request.BannerId);
            bannerDb.Id.Should().Be(BannerId);
            bannerDb.Title.Should().Be(Request.Title);
            bannerDb.Content.Should().Be(Request.Content);
            bannerDb.Target.Should().Be(Request.Target);
            bannerDb.Start.Should().Be(Request.Start);
            bannerDb.End.Should().Be(Request.End);
            bannerDb.IsOptedin.Should().BeTrue();
            bannerDb.CreatedBy.Should().Be(FirstBanner.CreatedBy);
            bannerDb.CreatedOn.Should().Be(FirstBanner.CreatedOn);
            bannerDb.ModifiedBy.Should().Be(Request.ModifiedBy);
            bannerDb.ModifiedOn.Should().Be(Now);
        }
    }
}