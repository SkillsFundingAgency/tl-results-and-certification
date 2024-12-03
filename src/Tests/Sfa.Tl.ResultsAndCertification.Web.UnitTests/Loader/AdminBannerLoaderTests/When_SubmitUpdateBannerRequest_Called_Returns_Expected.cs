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
    public class When_SubmitUpdateBannerRequest_Called_Returns_Expected : AdminBannerLoaderBaseTest
    {
        private readonly AdminEditBannerViewModel _request = new()
        {
            BannerId = 1,
            Title = "title",
            Content = "content",
            Target = BannerTarget.Both,
            IsActive = true,
            StartDay = "1",
            StartMonth = "1",
            StartYear = "2024",
            EndDay = "10",
            EndMonth = "7",
            EndYear = "2025"
        };

        private readonly bool _apiResponse = true;
        private bool _result;

        private readonly Expression<Predicate<UpdateBannerRequest>> _apiPredicate =
            r => r.BannerId == 1
            && r.Title == "title"
            && r.Content == "content"
            && r.Target == BannerTarget.Both
            && r.IsActive == true
            && r.Start == new DateTime(2024, 1, 1)
            && r.End == new DateTime(2025, 7, 10)
            && !string.IsNullOrEmpty(r.ModifiedBy);

        public override void Given()
        {
            ApiClient
                .UpdateBannerAsync(Arg.Is(_apiPredicate))
                .Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.SubmitUpdateBannerRequest(_request);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).UpdateBannerAsync(Arg.Is(_apiPredicate));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().Be(_apiResponse);
        }
    }
}