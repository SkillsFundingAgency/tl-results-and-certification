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
    public class When_GetSelectProviderTlevelsAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private Task<ProviderTlevels> _result;
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
                        new ProviderTlevel { TqAwardingOrganisationId = 1, TlProviderId = 1, RouteName = "Route1", PathwayName = "Pathway1"},
                        new ProviderTlevel { TqAwardingOrganisationId = 1, TlProviderId = 1, RouteName = "Route2", PathwayName = "Pathway2"}
                    }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<ProviderTlevels>(_mockHttpResult, string.Format(ApiConstants.GetSelectProviderTlevelsUri, _ukprn, _providerId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.GetSelectProviderTlevelsAsync(_ukprn, _providerId);
        }

        [Fact]
        public void Then_Two_Provider_Tlevels_Are_Returned()
        {
            _result.Result.Should().NotBeNull();
            _result.Result.Tlevels.Should().NotBeNull();
            _result.Result.Tlevels.Count().Should().Be(2);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            var actualResult = _result.Result;

            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(_mockHttpResult.Id);
            actualResult.DisplayName.Should().Be(_mockHttpResult.DisplayName);
            actualResult.Ukprn.Should().Be(_mockHttpResult.Ukprn);
            actualResult.Tlevels.Should().NotBeNull();

            var expectedTlevelResult = _mockHttpResult.Tlevels.FirstOrDefault();
            var actualProviderTlevelResult = actualResult.Tlevels.FirstOrDefault();
            actualProviderTlevelResult.Should().NotBeNull();

            actualProviderTlevelResult.TqAwardingOrganisationId.Should().Be(expectedTlevelResult.TqAwardingOrganisationId);
            actualProviderTlevelResult.PathwayName.Should().Be(expectedTlevelResult.PathwayName);
            actualProviderTlevelResult.RouteName.Should().Be(expectedTlevelResult.RouteName);
        }
    }
}
