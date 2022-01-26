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
    public class When_AddResult_For_Specialism_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected AddResultResponse _result;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected AddResultResponse _mockHttpResult;
        private AddResultRequest _model;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new AddResultResponse
            {
                IsSuccess = true,
                Uln = 1234567890,
                ProfileId = 11
            };

            _model = new AddResultRequest
            {
                AoUkprn = 1,
                ProfileId = 11,
                AssessmentId = 1,
                LookupId = 1,
                ComponentType = Common.Enum.ComponentType.Specialism,
                PerformedBy = "Test User"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<AddResultResponse>(_mockHttpResult, ApiConstants.AddResultUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.AddResultAsync(_model);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeTrue();
            _result.Uln.Should().Be(_mockHttpResult.Uln);
            _result.ProfileId.Should().Be(_mockHttpResult.ProfileId);
        }
    }
}
