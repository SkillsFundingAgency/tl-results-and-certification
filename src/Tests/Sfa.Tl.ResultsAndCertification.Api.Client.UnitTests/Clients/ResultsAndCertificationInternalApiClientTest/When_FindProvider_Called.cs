using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_FindProvider_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        // Dependencies
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        
        private Task<IEnumerable<ProviderMetadata>> _result;
        private IEnumerable<ProviderMetadata> _mockHttpResult;

        // Method Parameters
        private readonly string _name = "TestProvider";
        private readonly bool _isExactMatch = true;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://provider.api.com" }
            };

            _mockHttpResult = new List<ProviderMetadata>
            {
                new ProviderMetadata { Id = 1, DisplayName = "Test provider 1" },
                new ProviderMetadata { Id = 2, DisplayName = "Test provider 2" },
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IEnumerable<ProviderMetadata>>(_mockHttpResult, string.Format(ApiConstants.FindProviderAsyncUri, _name, _isExactMatch), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.FindProviderAsync(_name, _isExactMatch);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var actualResult = _result.Result;
            actualResult.Should().NotBeNull();
            actualResult.Count().Should().Be(2);

            actualResult.First().Id.Should().Be(_mockHttpResult.First().Id);
            actualResult.First().DisplayName.Should().Be(_mockHttpResult.First().DisplayName);
        }
    }
}
