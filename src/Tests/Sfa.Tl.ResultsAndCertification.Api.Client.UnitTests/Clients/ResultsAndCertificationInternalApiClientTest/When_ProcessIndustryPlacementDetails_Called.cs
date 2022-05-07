using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_ProcessIndustryPlacementDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;

        private IndustryPlacementRequest _request;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private bool _mockHttpResult;
        private bool _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = true;

            _request = new IndustryPlacementRequest
            {
                ProviderUkprn = 9876543210,
                ProfileId = 1,
                RegistrationPathwayId = 2,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed,
                IndustryPlacementDetails = new IndustryPlacementDetails
                {
                    IndustryPlacementStatus = "Completed",
                    IndustryPlacementModels = new List<int> { 1,2},
                    MultipleEmployerModelsUsed = true,
                    OtherIndustryPlacementModels = new List<int> { 3, 4},
                    TemporaryFlexibilitiesUsed = true,
                    BlendedTemporaryFlexibilityUsed = true,
                    TemporaryFlexibilities = new List<int> { 5, 6 }
                },
                PerformedBy = "John Smith"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<bool>(_mockHttpResult, ApiConstants.ProcessIndustryPlacementDetailsUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_request)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.ProcessIndustryPlacementDetailsAsync(_request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeTrue();
        }
    }
}
