using FluentAssertions;
using Newtonsoft.Json;
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
    public class When_AddBanner_Called : ResultsAndCertificationInternalApiClientBaseTest
    {
        private AddBannerResponse _result;

        private readonly AddBannerRequest _request = new()
        {
            Title = "title",
            Content = "content",
            Target = BannerTarget.Both,
            Start = new DateTime(2024, 1, 1),
            End = new DateTime(2024, 1, 2),
            CreatedBy = "test-user"
        };

        private readonly AddBannerResponse _mockHttpResult = new()
        {
            Success = true,
            BannerId = 1
        };

        private ResultsAndCertificationInternalApiClient _apiClient;

        public override void Given()
        {
            _apiClient = CreateApiClient(() => new MockHttpMessageHandler<AddBannerResponse>(_mockHttpResult, ApiConstants.AddBanner, HttpStatusCode.OK, JsonConvert.SerializeObject(_request)));
        }

        public async override Task When()
        {
            _result = await _apiClient.AddBannerAsync(_request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_mockHttpResult);
        }
    }
}