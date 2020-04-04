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
    public class When_GetTqProviderTlevelDetailsAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private Task<ProviderTlevelDetails> _result;
        private readonly long _ukprn = 12345678;
        private readonly int _tqProviderId = 1;
        protected ProviderTlevelDetails _mockHttpResult;
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new ProviderTlevelDetails
            {
                Id = 1,
                DisplayName = "Test",
                Ukprn = 123456,
                ProviderTlevel = new ProviderTlevel { RouteName = "Route", PathwayName = "Pathway"}
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<ProviderTlevelDetails>(_mockHttpResult, string.Format(ApiConstants.GetTqProviderTlevelDetailsAsyncUri, _ukprn, _tqProviderId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.GetTqProviderTlevelDetailsAsync(_ukprn, _tqProviderId);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            var actualResult = _result.Result;

            actualResult.Should().NotBeNull();

            actualResult.Id.Should().Be(_mockHttpResult.Id);
            actualResult.DisplayName.Should().Be(_mockHttpResult.DisplayName);
            actualResult.Ukprn.Should().Be(_mockHttpResult.Ukprn);
            actualResult.ProviderTlevel.RouteName.Should().Be(_mockHttpResult.ProviderTlevel.RouteName);
            actualResult.ProviderTlevel.PathwayName.Should().Be(_mockHttpResult.ProviderTlevel.PathwayName);
        }
    }
}
