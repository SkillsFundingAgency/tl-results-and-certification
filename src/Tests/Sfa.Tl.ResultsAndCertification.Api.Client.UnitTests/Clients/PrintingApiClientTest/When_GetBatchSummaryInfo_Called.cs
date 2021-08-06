using FluentAssertions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.PrintingApiClientTest
{
    public class When_GetBatchSummaryInfo_Called : BaseTest<PrintingApiClient>
    {
        private int _batchId; 
        private BatchSummaryResponse _result;
        protected PrintToken _mockTokenHttpResult;
        protected BatchSummaryResponse _mockHttpResult;
        private ResultsAndCertificationConfiguration _configuration;
        private PrintingApiClient _apiClient;

        public override void Setup()
        {
            _configuration = new ResultsAndCertificationConfiguration
            {
                PrintingApiSettings = new PrintingApiSettings { Uri = "http://apitest.taone.co.uk", Username = "test", Password = "test" }
            };

            _batchId = 1;            
            _mockTokenHttpResult = new PrintToken { Token = Guid.NewGuid().ToString() };
            _mockHttpResult = new BatchSummaryResponse
            {
                BatchSummary = new List<BatchSummary>
                {
                    new BatchSummary
                    {
                        BatchNumber = _batchId,
                        BatchDate = DateTime.UtcNow,
                        ProcessedDate = DateTime.UtcNow.AddDays(1),
                        PostalContactCount = 1,
                        TotalCertificateCount = 1,
                        Status = PrintingStatus.CollectedByCourier.GetDisplayName(),
                        StatusChangeDate = DateTime.UtcNow.AddDays(1)
                    }
                }
            };
        }

        public override void Given()
        {
            var mockHttpHandler = new MockHttpMessageHandler<PrintToken>(_mockTokenHttpResult, string.Format(ApiConstants.PrintingTokenUri, _configuration.PrintingApiSettings.Username, _configuration.PrintingApiSettings.Password), HttpStatusCode.OK);
            mockHttpHandler.AddHttpResponses(_mockHttpResult, string.Format(ApiConstants.PrintBatchSummaryRequestUri, _batchId, _mockTokenHttpResult.Token), HttpStatusCode.OK);

            HttpClient = new HttpClient(mockHttpHandler);
            _apiClient = new PrintingApiClient(HttpClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetBatchSummaryInfoAsync(_batchId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.BatchSummary.Should().BeEquivalentTo(_mockHttpResult.BatchSummary);
        }
    }
}
