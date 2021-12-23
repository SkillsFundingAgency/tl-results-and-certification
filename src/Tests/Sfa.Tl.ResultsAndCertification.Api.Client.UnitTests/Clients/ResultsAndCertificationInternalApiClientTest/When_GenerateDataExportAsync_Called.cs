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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class GenerateDataExportAsync : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly string _requestedBy = "Test User";
        private readonly DataExportType _dataExportType = DataExportType.Assessments;
        private IList<DataExportResponse> _mockHttpResult;
        private long _aoUkprn = 10011881;
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private IList<DataExportResponse> _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new List<DataExportResponse>
            {
                new DataExportResponse { BlobUniqueReference = Guid.NewGuid(), FileSize = 100, ComponentType = ComponentType.Core, IsDataFound = true },
                new DataExportResponse { BlobUniqueReference = Guid.NewGuid(), FileSize = 200, ComponentType = ComponentType.Specialism, IsDataFound = true },
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IList<DataExportResponse>>(_mockHttpResult, string.Format(ApiConstants.GetDataExportUri, _aoUkprn, (int)_dataExportType, _requestedBy), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GenerateDataExportAsync(_aoUkprn, _dataExportType, _requestedBy);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Count.Should().Be(_mockHttpResult.Count);

            for (int idx = 0; idx < _mockHttpResult.Count; idx++)
            {
                _result[idx].BlobUniqueReference.Should().Be(_mockHttpResult[idx].BlobUniqueReference);
                _result[idx].FileSize.Should().Be(_mockHttpResult[idx].FileSize);
                _result[idx].IsDataFound.Should().Be(_mockHttpResult[idx].IsDataFound);
                _result[idx].ComponentType.Should().Be(_mockHttpResult[idx].ComponentType);
            }
        } 
    }
}
