using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificatePrintingService
{
    public class When_Called_ProcessTrackBatch_With_Valid_Data : TestSetup
    {
        private List<int> _batchIds;
        private CertificatePrintingResponse _expectedResult;
        private TrackBatchResponse _apiResponse;

        public override void Given()
        {
            var batchId = 1;
            _batchIds = new List<int> { batchId };
            PrintingService.GetPendingItemsForTrackBatchAsync().Returns(_batchIds);

            _apiResponse = new TrackBatchResponse
            {
                DeliveryNotifications = new List<DeliveryNotification>
                {
                    new DeliveryNotification
                    {
                        BatchNumber = batchId,
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

            PrintingApiClient.GetTrackBatchInfoAsync(Arg.Any<int>()).Returns(_apiResponse);

            _expectedResult = new CertificatePrintingResponse { IsSuccess = true, TotalCount = 1, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 1 };
            PrintingService.UpdateTrackBatchResponsesAsync(Arg.Any<List<TrackBatchResponse>>()).Returns(_expectedResult);
        }

        public async override Task When()
        {
            ActualResult = await Service.ProcessTrackBatchAsync();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            PrintingService.Received(1).GetPendingItemsForTrackBatchAsync();
            PrintingApiClient.Received(1).GetTrackBatchInfoAsync(Arg.Any<int>());
            PrintingService.Received(1).UpdateTrackBatchResponsesAsync(Arg.Any<List<TrackBatchResponse>>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(_expectedResult.IsSuccess);
            ActualResult.TotalCount.Should().Be(_expectedResult.TotalCount);
            ActualResult.PrintingProcessedCount.Should().Be(_expectedResult.PrintingProcessedCount);
            ActualResult.ModifiedCount.Should().Be(_expectedResult.ModifiedCount);
            ActualResult.SavedCount.Should().Be(_expectedResult.SavedCount);
        }
    }
}