using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminBannerLoaderTests
{
    public class When_SubmitAddBannerRequest_Called_Returns_Expected : AdminBannerLoaderBaseTest
    {
        private readonly AdminAddBannerViewModel _request = new()
        {
            Title = "title",
            Content = "content",
            Target = BannerTarget.Both,
            StartDay = "1",
            StartMonth = "1",
            StartYear = "2024",
            EndDay = "10",
            EndMonth = "7",
            EndYear = "2025"
        };

        private readonly AddBannerResponse _apiResponse = new()
        {
            Success = true,
            BannerId = 1
        };

        private AddBannerResponse _result;

        private readonly Expression<Predicate<AddBannerRequest>> _apiPredicate =
            r => r.Title == "title"
            && r.Content == "content"
            && r.Target == BannerTarget.Both
            && r.Start == new DateTime(2024, 1, 1)
            && r.End == new DateTime(2025, 7, 10)
            && !string.IsNullOrEmpty(r.CreatedBy);

        public override void Given()
        {
            ApiClient
                .AddBannerAsync(Arg.Is(_apiPredicate))
                .Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.SubmitAddBannerRequest(_request);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).AddBannerAsync(Arg.Is(_apiPredicate));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_apiResponse);
        }
    }
}