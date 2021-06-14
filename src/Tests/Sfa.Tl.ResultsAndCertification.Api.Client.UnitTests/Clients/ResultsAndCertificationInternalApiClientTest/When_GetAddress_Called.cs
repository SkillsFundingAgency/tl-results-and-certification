using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetAddress_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        // inputs
        private readonly long _providerUkprn = 12345678;

        // results
        private Address _actualResult;
        private Address _mockApiResponse;

        // dependencies
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

            _mockApiResponse = new Address
            {
                DepartmentName = "Dept",
                OrganisationName = "Org",
                AddressLine1 = "Line1",
                AddressLine2 = "Line2",
                Town = "Town",
                Postcode = "x11 1yy"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<Address>(_mockApiResponse, string.Format(ApiConstants.GetAddressUri, _providerUkprn), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetAddressAsync(_providerUkprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.DepartmentName.Should().Be(_mockApiResponse.DepartmentName);
            _actualResult.OrganisationName.Should().Be(_mockApiResponse.OrganisationName);
            _actualResult.AddressLine1.Should().Be(_mockApiResponse.AddressLine1);
            _actualResult.AddressLine2.Should().Be(_mockApiResponse.AddressLine2);
            _actualResult.Town.Should().Be(_mockApiResponse.Town);
            _actualResult.Postcode.Should().Be(_mockApiResponse.Postcode);            
        }
    }
}
