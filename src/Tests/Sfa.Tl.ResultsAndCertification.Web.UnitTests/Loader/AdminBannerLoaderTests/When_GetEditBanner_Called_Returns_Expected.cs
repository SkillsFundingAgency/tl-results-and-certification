using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminBannerLoaderTests
{
    public class When_GetEditBanner_Called_Returns_Expected : AdminBannerLoaderBaseTest
    {
        private const int BannerId = 1;

        private readonly GetBannerResponse _apiResponse = new()
        {
            Id = 1,
            Title = "title",
            Content = "content",
            Target = BannerTarget.AwardingOrganisation,
            Start = new DateTime(2024, 12, 1),
            End = new DateTime(2024, 12, 31),
            IsOptedin = true,
        };

        private AdminEditBannerViewModel _result;

        public override void Given()
        {
            ApiClient.GetBannerAsync(BannerId).Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.GetEditBannerViewModel(BannerId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetBannerAsync(BannerId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            _result.BannerId.Should().Be(_apiResponse.Id);
            _result.Title.Should().Be(_apiResponse.Title);
            _result.Content.Should().Be(_apiResponse.Content);
            _result.Target.Should().Be(_apiResponse.Target);
            _result.IsActive.Should().Be(_apiResponse.IsOptedin);

            _result.StartDay.Should().Be(_apiResponse.Start.Day.ToString());
            _result.StartMonth.Should().Be(_apiResponse.Start.Month.ToString());
            _result.StartYear.Should().Be(_apiResponse.Start.Year.ToString());

            _result.EndDay.Should().Be(_apiResponse.End.Day.ToString());
            _result.EndMonth.Should().Be(_apiResponse.End.Month.ToString());
            _result.EndYear.Should().Be(_apiResponse.End.Year.ToString());
        }
    }
}