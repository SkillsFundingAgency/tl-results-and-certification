using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminBannerLoaderTests
{
    public class When_GetBannerDetails_Called_Returns_Expected : AdminBannerLoaderBaseTest
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

        private AdminBannerDetailsViewModel _result;

        public override void Given()
        {
            ApiClient.GetBannerAsync(BannerId).Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.GetBannerDetailsViewModel(BannerId);
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

            _result.SummaryTarget.Id.Should().Be("target");
            _result.SummaryTarget.Title.Should().Be(AdminBannerDetails.Label_Target);
            _result.SummaryTarget.Value.Should().Be("Awarding organisation");

            _result.SummaryIsActive.Id.Should().Be("active");
            _result.SummaryIsActive.Title.Should().Be(AdminBannerDetails.Label_Active);
            _result.SummaryIsActive.Value.Should().Be(AdminBannerDetails.Label_Yes);

            _result.SummaryStartDate.Id.Should().Be("startdate");
            _result.SummaryStartDate.Title.Should().Be(AdminBannerDetails.Label_Start_Date);
            _result.SummaryStartDate.Value.Should().Be(_apiResponse.Start.ToDobFormat());

            _result.SummaryEndDate.Id.Should().Be("enddate");
            _result.SummaryEndDate.Title.Should().Be(AdminBannerDetails.Label_End_Date);
            _result.SummaryEndDate.Value.Should().Be(_apiResponse.End.ToDobFormat());

            _result.BackLink.RouteName.Should().Be(RouteConstants.AdminFindBanner);
            _result.BackLink.RouteAttributes.Should().BeEmpty();

            _result.SuccessBanner.Should().BeNull();
        }
    }
}