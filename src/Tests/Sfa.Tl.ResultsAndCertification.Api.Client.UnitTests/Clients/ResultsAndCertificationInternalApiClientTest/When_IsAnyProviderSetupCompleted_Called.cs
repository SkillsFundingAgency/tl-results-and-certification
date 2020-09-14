using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_IsAnyProviderSetupCompleted_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        // Dependencies
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private bool _result;

        private bool _mockHttpResult;

        // Method Parameters
        private readonly long _ukprn = 12345678;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://provider.api.com" }
            };

            _mockHttpResult = true;
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<bool>(_mockHttpResult, string.Format(ApiConstants.IsAnyProviderSetupCompletedUri, _ukprn), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.IsAnyProviderSetupCompletedAsync(_ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeTrue();
        }
    }
}
