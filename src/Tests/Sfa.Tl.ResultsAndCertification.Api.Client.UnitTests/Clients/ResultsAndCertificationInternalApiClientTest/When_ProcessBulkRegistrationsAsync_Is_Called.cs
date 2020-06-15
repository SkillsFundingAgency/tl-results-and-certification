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
    public class When_ProcessBulkRegistrationsAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected Task<BulkRegistrationResponse> Result;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected BulkRegistrationResponse _mockHttpResult;
        private BulkRegistrationRequest _model;
        private readonly long _ukprn = 12345678;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new BulkRegistrationResponse
            {
                IsSuccess = false,
                BlobUniqueReference = Guid.NewGuid(),
                ErrorFileSize = 1.5
            };

            _model = new BulkRegistrationRequest
            {
                AoUkprn = _ukprn,
                BlobFileName = "inputfile_1.csv",
                BlobUniqueReference = Guid.NewGuid()
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<BulkRegistrationResponse>(_mockHttpResult, ApiConstants.ProcessBulkRegistrationsUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            Result = _apiClient.ProcessBulkRegistrationsAsync(_model);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            var actualResult = Result.Result;

            actualResult.Should().NotBeNull();

            actualResult.IsSuccess.Should().BeFalse();
            actualResult.BlobUniqueReference.Should().Be(_mockHttpResult.BlobUniqueReference);
            actualResult.ErrorFileSize.Should().Be(_mockHttpResult.ErrorFileSize);
        }
    }
}
