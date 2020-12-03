using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using FluentAssertions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_AddAssessmentEntry_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected AddAssessmentEntryResponse _result;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected AddAssessmentEntryResponse _mockHttpResult;
        private AddAssessmentEntryRequest _model;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new AddAssessmentEntryResponse
            {
                IsSuccess = true,
                UniqueLearnerNumber = 1234567890
            };

            _model = new AddAssessmentEntryRequest
            {
                AoUkprn = 1,
                ProfileId = 11,
                AssessmentSeriesId = 111, 
                AssessmentEntryType = Common.Enum.AssessmentEntryType.Core,
                PerformedBy = "Test User"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<AddAssessmentEntryResponse>(_mockHttpResult, ApiConstants.AddAssessmentEntryUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.AddAssessmentEntryAsync(_model);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeTrue();
            _result.UniqueLearnerNumber.Should().Be(_mockHttpResult.UniqueLearnerNumber);
        }
    }
}
