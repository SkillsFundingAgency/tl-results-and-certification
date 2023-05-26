using FluentAssertions;
using Newtonsoft.Json;
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
    public class When_SetRegistrationAsPendingWithdrawal_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private bool _result;
        protected bool _mockHttpResult;
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private SetRegistrationAsPendingWithdrawalRequest _model;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = true;
            _model = new SetRegistrationAsPendingWithdrawalRequest
            {
                ProfileId = 1,
                ProviderUkprn = 1234567890,
                PerformedBy = "Test User"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<bool>(_mockHttpResult, ApiConstants.SetRegistrationAsPendingWithdrawalUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.SetRegistrationAsPendingWithdrawalAsync(_model);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeTrue();
        }
    }
}
