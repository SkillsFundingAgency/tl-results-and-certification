using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.PrintingApiClientTest
{
    public class When_GetToken_Called : BaseTest<PrintingApiClient>
    {
        private string _result;
        protected PrintToken _mockHttpResult;
        private ResultsAndCertificationConfiguration _configuration;
        private PrintingApiClient _apiClient;        

        public override void Setup()
        {
            _configuration = new ResultsAndCertificationConfiguration
            {
                PrintingApiSettings = new PrintingApiSettings { Uri = "http://apitest.taone.co.uk", Username = "test", Password = "test" }
            };

            _mockHttpResult = new PrintToken { Token = Guid.NewGuid().ToString() };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<PrintToken>(_mockHttpResult, string.Format(ApiConstants.PrintingTokenUri, _configuration.PrintingApiSettings.Username, _configuration.PrintingApiSettings.Password), HttpStatusCode.OK));
            _apiClient = new PrintingApiClient(HttpClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetTokenAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNullOrEmpty();
            _result.Should().Be(_mockHttpResult.Token);
        }
    }
}
