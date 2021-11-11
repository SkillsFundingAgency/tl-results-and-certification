using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.UcasApiClientTest
{
    public class When_GetToken_Called : BaseTest<UcasApiClient>
    {
        private string _result;
        protected UcasTokenResponse _mockHttpResult;
        private ResultsAndCertificationConfiguration _configuration;
        private UcasApiClient _apiClient;

        public override void Setup()
        {
            _configuration = new ResultsAndCertificationConfiguration
            {
                UcasApiSettings = new UcasApiSettings { Uri = "https://transfer.ucasenvironments.com/", Username = "test", Password = "test", Version = 1, FolderId = "12345", GrantType = "pass" }
            };

            _mockHttpResult = new UcasTokenResponse { AccessToken = Guid.NewGuid().ToString() };
        }

        public override void Given()
        {
            string requestParameters = string.Format(ApiConstants.UcasTokenParameters, _configuration.UcasApiSettings.GrantType, _configuration.UcasApiSettings.Username, _configuration.UcasApiSettings.Password);
            HttpClient = new HttpClient(new MockHttpMessageHandler<UcasTokenResponse>(_mockHttpResult, $"{string.Format(ApiConstants.UcasBaseUri, _configuration.UcasApiSettings.Version)}{ApiConstants.UcasTokenUri}", HttpStatusCode.OK, requestParameters));
            _apiClient = new UcasApiClient(HttpClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetTokenAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNullOrEmpty();
            _result.Should().Be(_mockHttpResult.AccessToken);
        }
    }
}
