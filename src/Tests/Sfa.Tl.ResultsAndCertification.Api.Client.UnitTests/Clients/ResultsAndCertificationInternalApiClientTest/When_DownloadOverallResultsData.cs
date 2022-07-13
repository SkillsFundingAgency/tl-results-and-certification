using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_DownloadOverallResultsData : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly string _requestedBy = "Test User";
        private DataExportResponse _mockHttpResult;
        private long _providerUkprn = 10011881;
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private DataExportResponse _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new DataExportResponse { BlobUniqueReference = Guid.NewGuid(), FileSize = 100, ComponentType = ComponentType.Core, IsDataFound = true };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<DataExportResponse>(_mockHttpResult, string.Format(ApiConstants.DownloadOverallResultsDataUri, _providerUkprn, _requestedBy), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.DownloadOverallResultsDataAsync(_providerUkprn, _requestedBy);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.BlobUniqueReference.Should().Be(_mockHttpResult.BlobUniqueReference);
            _result.FileSize.Should().Be(_mockHttpResult.FileSize);
            _result.IsDataFound.Should().Be(_mockHttpResult.IsDataFound);
            _result.ComponentType.Should().Be(_mockHttpResult.ComponentType);
        } 
    }
}
