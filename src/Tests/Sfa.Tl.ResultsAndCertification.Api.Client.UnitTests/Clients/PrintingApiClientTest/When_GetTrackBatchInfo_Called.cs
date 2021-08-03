using FluentAssertions;
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
        private int _batchId;
        private TrackBatchResponse _result;
        protected PrintToken _mockTokenHttpResult;
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
            _mockTokenHttpResult = new PrintToken { Token = Guid.NewGuid().ToString() };
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
                                TrackingId = "4578YA879153637",
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
            
            var mockHttpHandler = new MockHttpMessageHandler<PrintToken>(_mockTokenHttpResult, string.Format(ApiConstants.PrintingTokenUri, _configuration.PrintingApiSettings.Username, _configuration.PrintingApiSettings.Password), HttpStatusCode.OK);
            mockHttpHandler.AddHttpResponses(_mockHttpResult, string.Format(ApiConstants.PrintTrackBatchRequestUri, _batchId, _mockTokenHttpResult.Token), HttpStatusCode.OK);

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
