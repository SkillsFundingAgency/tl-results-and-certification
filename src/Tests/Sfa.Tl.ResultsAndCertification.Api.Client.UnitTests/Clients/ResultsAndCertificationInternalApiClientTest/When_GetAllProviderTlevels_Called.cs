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
    public class When_GetAllProviderTlevels_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ProviderTlevels _result;
        private readonly long _ukprn = 12345678;
        private readonly int _providerId = 1;
        protected ProviderTlevels _mockHttpResult;
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

            _mockHttpResult = new ProviderTlevels
            {
                Id = 1,
                DisplayName = "Test1",
                Ukprn = _ukprn,
                Tlevels = new List<ProviderTlevel>
                    {
                        new ProviderTlevel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Tlevel Title1"},
                        new ProviderTlevel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Tlevel Title2"}
                    }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<ProviderTlevels>(_mockHttpResult, string.Format(ApiConstants.GetAllProviderTlevelsAsyncUri, _ukprn, _providerId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetAllProviderTlevelsAsync(_ukprn, _providerId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {           
            _result.Should().NotBeNull();
            _result.Id.Should().Be(_mockHttpResult.Id);
            _result.DisplayName.Should().Be(_mockHttpResult.DisplayName);
            _result.Ukprn.Should().Be(_mockHttpResult.Ukprn);
            _result.Tlevels.Should().NotBeNull();
            _result.Tlevels.Count().Should().Be(2);

            var expectedTlevelResult = _mockHttpResult.Tlevels.FirstOrDefault();
            var actualProviderTlevelResult = _result.Tlevels.FirstOrDefault();
            actualProviderTlevelResult.Should().NotBeNull();

            actualProviderTlevelResult.TqAwardingOrganisationId.Should().Be(expectedTlevelResult.TqAwardingOrganisationId);
            actualProviderTlevelResult.TlevelTitle.Should().Be(expectedTlevelResult.TlevelTitle);
        }
    }
}
