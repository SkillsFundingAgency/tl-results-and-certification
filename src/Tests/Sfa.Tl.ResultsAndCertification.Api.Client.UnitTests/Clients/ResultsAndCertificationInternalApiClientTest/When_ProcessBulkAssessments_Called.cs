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
using System;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_ProcessBulkAssessments_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected BulkAssessmentResponse _result;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected BulkAssessmentResponse _mockHttpResult;
        private BulkRegistrationRequest _model;
        private readonly long _ukprn = 12345678;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new BulkAssessmentResponse
            {
                IsSuccess = false,
                BlobUniqueReference = Guid.NewGuid(),
                ErrorFileSize = 1.5
            };

            _model = new BulkRegistrationRequest
            {
                AoUkprn = _ukprn,
                BlobFileName = "assessmenttfile.csv",
                BlobUniqueReference = Guid.NewGuid()
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<BulkAssessmentResponse>(_mockHttpResult, ApiConstants.ProcessBulkAssessmentsUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.ProcessBulkAssessmentsAsync(_model);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeFalse();
            _result.BlobUniqueReference.Should().Be(_mockHttpResult.BlobUniqueReference);
            _result.ErrorFileSize.Should().Be(_mockHttpResult.ErrorFileSize);
        }
    }
}
