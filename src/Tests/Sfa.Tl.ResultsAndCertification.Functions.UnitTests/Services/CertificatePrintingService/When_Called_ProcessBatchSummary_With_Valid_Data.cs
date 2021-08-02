using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificatePrintingService
{
    public class When_Called_ProcessBatchSummary_With_Valid_Data : TestSetup
    {
        private List<int> _batchIds;
        private CertificatePrintingResponse _expectedResult;
        private BatchSummaryResponse _apiResponse;

        public override void Given()
        {
            var batchId = 1;
            _batchIds = new List<int> { batchId };
            PrintingService.GetPendingPrintBatchesForBatchSummaryAsync().Returns(_batchIds);

            _apiResponse = new BatchSummaryResponse 
            { 
                BatchSummary = new List<BatchSummary> 
                { 
                    new BatchSummary 
                    {
                        BatchNumber = batchId,
                        BatchDate = DateTime.UtcNow,
                        ProcessedDate = DateTime.UtcNow.AddDays(1),
                        PostalContactCount = 1,
                        TotalCertificateCount = 1,
                        Status = PrintingStatus.CollectedByCourier.GetDisplayName(),
                        StatusChangeDate = DateTime.UtcNow.AddDays(1)
                    }
                }
            };
            PrintingApiClient.GetBatchSummaryInfoAsync(Arg.Any<int>()).Returns(_apiResponse);

            _expectedResult = new CertificatePrintingResponse { IsSuccess = true, TotalCount = 1, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 1 };
            PrintingService.UpdateBatchSummaryResponsesAsync(Arg.Any<List<BatchSummaryResponse>>()).Returns(_expectedResult);
        }

        public async override Task When()
        {
            ActualResult = await Service.ProcessBatchSummaryAsync();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            PrintingService.Received(1).GetPendingPrintBatchesForBatchSummaryAsync();
            PrintingApiClient.Received(1).GetBatchSummaryInfoAsync(Arg.Any<int>());
            PrintingService.Received(1).UpdateBatchSummaryResponsesAsync(Arg.Any<List<BatchSummaryResponse>>());
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
