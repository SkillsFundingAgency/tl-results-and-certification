using FluentAssertions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
    public class When_GetTrackBatchInfo_Called : BaseTest<PrintingApiClient>
    {
        private string _apiToken;        
        private int _batchId;
        private TrackBatchResponse _result;
        protected string _mockTokenHttpResult;
        protected TrackBatchResponse _mockHttpResult;
        private PrintingApiClient _apiClient;
        private ResultsAndCertificationConfiguration _configuration;

        public override void Setup()
        {
            _configuration = new ResultsAndCertificationConfiguration
            {
                PrintingApiSettings = new PrintingApiSettings { Uri = "http://apitest.taone.co.uk", Username = "test", Password = "test" }
            };

            _batchId = 1;
            _apiToken = "1a62f29845ac4fb4bd596b12f40e6881";
            _mockTokenHttpResult = "{\"Token\" : \"1a62f29845ac4fb4bd596b12f40e6881\"}";
            _mockHttpResult = new TrackBatchResponse
            {
                DeliveryNotifications = new List<DeliveryNotification>
                {
                    new DeliveryNotification
                    {
                        BatchNumber = _batchId,
                        TrackingDetails = new List<TrackingDetail>
                        {
                            new TrackingDetail
                            {
                                Name = "Barnsley College",
                                UKPRN = "98564231",
                                Status = PrintingBatchItemStatus.Delivered.ToString(),
                                Reason = string.Empty,
                                SignedForBy = string.Empty,
                                TrackingID = "4578YA879153637",
                                StatusChangeDate = DateTime.UtcNow
                            }
                        },
                        Status = ResponseStatus.Success.ToString(),
                        ErrorMessage = string.Empty
                    }
                }
            };
        }

        public override void Given()
        {
            var jsonMockHttpResult = JsonConvert.SerializeObject(_mockHttpResult);
            var mockHttpHandler = new MockHttpMessageHandler<string>(_mockTokenHttpResult, string.Format(ApiConstants.PrintingTokenUri, _configuration.PrintingApiSettings.Username, _configuration.PrintingApiSettings.Password), HttpStatusCode.OK);
            mockHttpHandler.AddHttpResponses(jsonMockHttpResult, string.Format(ApiConstants.PrintTrackBatchRequestUri, _batchId, _apiToken), HttpStatusCode.OK);

            HttpClient = new HttpClient(mockHttpHandler);
            _apiClient = new PrintingApiClient(HttpClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetTrackBatchInfoAsync(_batchId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.DeliveryNotifications.Should().BeEquivalentTo(_mockHttpResult.DeliveryNotifications);
        }
    }
}
