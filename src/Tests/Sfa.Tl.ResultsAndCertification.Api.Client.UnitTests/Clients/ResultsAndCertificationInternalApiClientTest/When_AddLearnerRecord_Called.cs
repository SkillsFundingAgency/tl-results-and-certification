using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_AddLearnerRecord_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private AddLearnerRecordResponse _result;
        protected AddLearnerRecordResponse _mockHttpResult;
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private AddLearnerRecordRequest _model;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new AddLearnerRecordResponse
            {
                Uln = 1234567891,
                Name = "Test User",
                IsSuccess = true
            };

            _model = new AddLearnerRecordRequest
            {
                Ukprn = 58974561,
                Uln = 1234567891,
                HasLrsEnglishAndMaths = false,
                EnglishAndMathsStatus = EnglishAndMathsStatus.AchievedWithSend,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed,
                PerformedBy = "Test User"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<AddLearnerRecordResponse>(_mockHttpResult, ApiConstants.AddLearnerRecordUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.AddLearnerRecordAsync(_model);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Uln.Should().Be(_mockHttpResult.Uln);
            _result.Name.Should().Be(_mockHttpResult.Name);
            _result.IsSuccess.Should().Be(_mockHttpResult.IsSuccess);
        }
    }
}
