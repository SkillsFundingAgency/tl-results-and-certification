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
using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetDocumentUploadHistoryDetails_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected DocumentUploadHistoryDetails Result;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected DocumentUploadHistoryDetails _mockHttpResult;
        private readonly long _ukprn = 12345678;
        private readonly Guid _blobUniqueReference = Guid.NewGuid();

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new DocumentUploadHistoryDetails
            {
                TlAwardingOrganisationId = 1,
                AoUkprn = _ukprn,
                BlobUniqueReference = Guid.NewGuid(),
                BlobFileName = "inputfile.csv",
                DocumentType = (int)DocumentType.Registrations,
                FileType = (int)FileType.Csv,
                Status = (int)BulkRegistrationProcessStatus.Processed
            };            
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<DocumentUploadHistoryDetails>(_mockHttpResult, string.Format(ApiConstants.GetDocumentUploadHistoryDetailsAsyncUri, _ukprn, _blobUniqueReference), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            Result = await _apiClient.GetDocumentUploadHistoryDetailsAsync(_ukprn, _blobUniqueReference);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.TlAwardingOrganisationId.Should().Be(_mockHttpResult.TlAwardingOrganisationId);
            Result.AoUkprn.Should().Be(_mockHttpResult.AoUkprn);
            Result.BlobUniqueReference.Should().Be(_mockHttpResult.BlobUniqueReference);
            Result.BlobFileName.Should().Be(_mockHttpResult.BlobFileName);
            Result.DocumentType.Should().Be(_mockHttpResult.DocumentType);
            Result.FileType.Should().Be(_mockHttpResult.FileType);
            Result.Status.Should().Be(_mockHttpResult.Status);
        }
    }
}
