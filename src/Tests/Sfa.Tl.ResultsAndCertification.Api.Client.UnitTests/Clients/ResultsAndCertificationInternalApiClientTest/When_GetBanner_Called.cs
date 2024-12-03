using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetBanner_Called : ResultsAndCertificationInternalApiClientBaseTest
    {
        private const int BannerId = 1;
        private GetBannerResponse _result;

        private readonly GetBannerResponse _mockHttpResult = new()
        {
            Id = BannerId,
            Title = "title",
            Content = "content",
            Target = BannerTarget.Both,
            IsOptedin = true,
            Start = new DateTime(2024, 1, 1),
            End = new DateTime(2024, 1, 2)
        };

        private ResultsAndCertificationInternalApiClient _apiClient;

        public override void Given()
        {
            _apiClient = CreateApiClient(() => new MockHttpMessageHandler<GetBannerResponse>(_mockHttpResult, string.Format(ApiConstants.GetBanner, BannerId), HttpStatusCode.OK));
        }

        public async override Task When()
        {
            _result = await _apiClient.GetBannerAsync(BannerId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_mockHttpResult);
        }
    }
}