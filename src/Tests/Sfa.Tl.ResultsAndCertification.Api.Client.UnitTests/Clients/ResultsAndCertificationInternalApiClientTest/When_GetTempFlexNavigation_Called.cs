using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetTempFlexNavigation_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        
        private readonly int _pathwayId = 1;
        private readonly int _academicYear = 2020;

        private ResultsAndCertificationInternalApiClient _apiClient;
        private IpTempFlexNavigation _mockHttpResult;
        private IpTempFlexNavigation _actualResult;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new IpTempFlexNavigation { AskTempFlexibility = true, AskBlendedPlacement = true };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IpTempFlexNavigation>(_mockHttpResult, string.Format(ApiConstants.GetTempFlexNavigationUri, _pathwayId, _academicYear), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _actualResult = await _apiClient.GetTempFlexNavigationAsync(_pathwayId, _academicYear);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.AskBlendedPlacement.Should().Be(_mockHttpResult.AskBlendedPlacement);
            _actualResult.AskTempFlexibility.Should().Be(_mockHttpResult.AskTempFlexibility);
        }
    }
}
