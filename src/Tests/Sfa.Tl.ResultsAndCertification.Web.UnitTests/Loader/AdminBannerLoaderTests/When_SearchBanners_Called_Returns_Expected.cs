using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminBannerLoaderTests
{
    public class When_SearchBanners_Called_Returns_Expected : AdminBannerLoaderBaseTest
    {
        private readonly AdminSearchBannerRequest _request = new()
        {
            SelectActiveBanners = true,
            SelectedTargets = new[] { BannerTarget.AwardingOrganisation },
            PageNumber = 1
        };

        private readonly PagedResponse<SearchBannerDetail> _apiResponse = new()
        {
            Records = new[]
            {
                new SearchBannerDetail
                {
                    Id = 1,
                    Title = "title",
                    Content = "content",
                    Target = BannerTarget.AwardingOrganisation,
                    IsOptedin = true,
                    Start = new DateTime(2024, 12, 1),
                    End = new DateTime(2024, 12, 31)
                }
            },
            TotalRecords = 1,
            PagerInfo = new Pager(1, 1, 10)
        };

        private AdminFindBannerViewModel _result;

        public override void Given()
        {
            ApiClient.SearchBannersAsync(_request).Returns(_apiResponse);
        }

        public override async Task When()
        {
            //_result = await Loader.SearchBannersAsync(_request);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).SearchBannersAsync(_request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            SearchBannerDetail bannerDetail = _apiResponse.Records.First();

            _result.Details.Should().HaveCount(1);
            _result.Details[0].Content.Should().Be(bannerDetail.Content);
            _result.Details[0].Target.Should().Be("Awarding organisation");
            _result.Details[0].Active.Should().Be(bannerDetail.IsOptedin);

            ChangeRecordModel link = _result.Details[0].BannerDetailsLink;
            link.Text.Should().Be(bannerDetail.Title);
            link.Route.Should().Be(RouteConstants.AdminBannerDetails);
            link.RouteAttributes.Should().HaveCount(1);
            link.RouteAttributes.Should().Contain(Constants.BannerId, bannerDetail.Id.ToString());

            _result.TotalRecords.Should().Be(1);

            PagerViewModel pagerInfo = _result.Pagination.PagerInfo;
            pagerInfo.TotalItems.Should().Be(1);
            pagerInfo.CurrentPage.Should().Be(1);
            pagerInfo.PageSize.Should().Be(10);
            pagerInfo.TotalPages.Should().Be(1);
            pagerInfo.StartPage.Should().Be(1);
            pagerInfo.RecordFrom.Should().Be(1);
            pagerInfo.RecordTo.Should().Be(1);
        }
    }
}