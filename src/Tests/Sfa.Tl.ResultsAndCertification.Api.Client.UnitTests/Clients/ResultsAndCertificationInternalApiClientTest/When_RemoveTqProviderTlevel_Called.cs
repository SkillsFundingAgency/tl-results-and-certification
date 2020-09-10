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
    public class When_RemoveTqProviderTlevel_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private readonly long _ukprn = 1024;
        private readonly int _tqProviderId = 1;
        private Task<bool> _result;

        private ResultsAndCertificationInternalApiClient _apiClient;
        private bool _mockHttpResult;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };
            _mockHttpResult = true;
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<bool>(_mockHttpResult, string.Format(ApiConstants.RemoveTqProviderTlevelAsyncUri, _ukprn, _tqProviderId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.RemoveTqProviderTlevelAsync(_ukprn, _tqProviderId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Result.Should().BeTrue();
        }
    }
}
