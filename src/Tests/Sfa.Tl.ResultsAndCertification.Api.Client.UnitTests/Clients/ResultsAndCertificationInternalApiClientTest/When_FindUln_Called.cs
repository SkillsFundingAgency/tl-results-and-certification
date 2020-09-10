using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_FindUln_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly long _ukprn = 12345678;
        private readonly long _uln = 987654321;
        
        protected FindUlnResponse _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private Task<FindUlnResponse> _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new FindUlnResponse
            {
                RegistrationProfileId = 1, 
                Uln = _uln,
                IsRegisteredWithOtherAo = true,
                IsActive = false
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<FindUlnResponse>(_mockHttpResult, string.Format(ApiConstants.FindUlnUri, _ukprn, _uln), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.FindUlnAsync(_ukprn, _uln);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var actualResult = _result.Result;

            actualResult.Should().NotBeNull();
            actualResult.Uln.Should().Be(_mockHttpResult.Uln);
            actualResult.RegistrationProfileId.Should().Be(_mockHttpResult.RegistrationProfileId);
            actualResult.IsRegisteredWithOtherAo.Should().Be(_mockHttpResult.IsRegisteredWithOtherAo);
            actualResult.IsActive.Should().Be(_mockHttpResult.IsActive);
        }
    }
}
