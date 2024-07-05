using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderRegistrations;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetProviderRegistrations_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly GetProviderRegistrationsRequest _request = new()
        {
            ProviderUkprn = 1,
            StartYear = 2020,
            RequestedBy = "test-user"
        };

        private DataExportResponse _actualResult;
        private DataExportResponse _mockApiResponse;

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

            _mockApiResponse = new DataExportResponse
            {

                FileSize = 100,
                BlobUniqueReference = new Guid("f2e1a7c3-6e4b-4b0d-9e8a-8a2c1e0f3b5e"),
                ComponentType = ComponentType.NotSpecified,
                IsDataFound = true
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<DataExportResponse>(_mockApiResponse, ApiConstants.GetRegistrationsUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_request)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetProviderRegistrationsAsync(_request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_mockApiResponse);
        }
    }
}