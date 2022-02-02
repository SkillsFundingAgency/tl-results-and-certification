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
    public class When_ChangeResult_For_Specialism_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected ChangeResultResponse _result;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected ChangeResultResponse _mockHttpResult;
        private ChangeResultRequest _model;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new ChangeResultResponse
            {
                IsSuccess = true,
                Uln = 1234567890,
                ProfileId = 7
            };

            _model = new ChangeResultRequest
            {
                AoUkprn = 89562378,
                ProfileId = 7,
                ResultId = 1,
                LookupId = 3,
                ComponentType = Common.Enum.ComponentType.Specialism,
                PerformedBy = "Test User"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<ChangeResultResponse>(_mockHttpResult, ApiConstants.ChangeResultUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.ChangeResultAsync(_model);
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
