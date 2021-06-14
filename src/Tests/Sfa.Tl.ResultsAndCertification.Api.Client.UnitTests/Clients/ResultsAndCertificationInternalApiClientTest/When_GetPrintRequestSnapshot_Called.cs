using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetPrintRequestSnapshot_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        // inputs
        private readonly long _providerUkprn = 12345678;
        private readonly int _profileId = 1;
        private readonly int _pathwayId = 10; 

        // results
        private PrintRequestSnapshot _actualResult;
        private PrintRequestSnapshot _mockApiResponse;

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

            _mockApiResponse = new PrintRequestSnapshot
            {
                RegistrationPathwayStatus = RegistrationPathwayStatus.Withdrawn,
                RequestedBy = "John Smith",
                RequestedOn = DateTime.Today,
                RequestDetails = "{\"Uln\":1234567890,\"Name\":\"First 1 Last 1\",\"Dateofbirth\":\"01 January 2006\",\"ProviderName\":\"Barnsley College (10000536)\",\"TlevelTitle\":\"T Level in Healthcare Science\",\"Core\":\"Healthcare Science (10923456)\",\"CoreGrade\":\"B\",\"Specialism\":\"Optical Care Services (38234567)\",\"SpecialismGrade\":\"None\",\"EnglishAndMaths\":\"Achieved minimum standard (Data from the Learning Records Service - LRS)\",\"IndustryPlacement\":\"Placement completed\",\"ProviderAddress\":{\"AddressId\":1,\"DepartmentName\":\"Operations\",\"OrganisationName\":\"College Ltd\",\"AddressLine1\":\"10, House\",\"AddressLine2\":\"Street\",\"Town\":\"Birmingham\",\"Postcode\":\"B1 1AA\"}}"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<PrintRequestSnapshot>(_mockApiResponse, string.Format(ApiConstants.GetPrintRequestSnapshotUri, _providerUkprn, _profileId, _pathwayId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetPrintRequestSnapshotAsync(_providerUkprn, _profileId, _pathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.RegistrationPathwayStatus.Should().Be(_mockApiResponse.RegistrationPathwayStatus);
            _actualResult.RequestedOn.Should().Be(_mockApiResponse.RequestedOn);
            _actualResult.RequestedBy.Should().Be(_mockApiResponse.RequestedBy);
            _actualResult.RequestDetails.Should().Be(_mockApiResponse.RequestDetails);
        }
    }
}
