using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using System;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminBannerServiceTests.AddBannerTests
{
    public class When_IsCalled_Returns_Expected : AddBannerBaseTest
    {
        public override void Given()
        {
            Request = new AddBannerRequest
            {
                Title = "new-banner-title",
                Content = "new-banner-content",
                Target = BannerTarget.Provider,
                Start = new DateTime(2024, 1, 1),
                End = new DateTime(2024, 1, 1),
                CreatedBy = "test-user"
            };

            SeedTestData();
        }

        [Fact]
        public void Then_Should_Return_True()
        {
            int bannerId = BannersDb.Max(b => b.Id) + 1;

            Result.Success.Should().BeTrue();
            Result.BannerId.Should().Be(bannerId);

            Banner bannerDb = DbContext.Banner.Single(b => b.Title == Request.Title);
            bannerDb.Id.Should().Be(bannerId);
            bannerDb.Title.Should().Be(Request.Title);
            bannerDb.Content.Should().Be(Request.Content);
            bannerDb.Target.Should().Be(Request.Target);
            bannerDb.Start.Should().Be(Request.Start);
            bannerDb.End.Should().Be(Request.End);
            bannerDb.IsOptedin.Should().BeTrue();
            bannerDb.CreatedBy.Should().Be(Request.CreatedBy);
            bannerDb.ModifiedBy.Should().BeNull();
            bannerDb.ModifiedOn.Should().BeNull();
        }
    }
}