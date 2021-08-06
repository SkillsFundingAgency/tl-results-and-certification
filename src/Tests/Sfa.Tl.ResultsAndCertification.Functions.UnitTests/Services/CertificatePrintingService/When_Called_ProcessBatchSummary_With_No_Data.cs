using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificatePrintingService
{
    public class When_Called_ProcessBatchSummary_With_No_Data : TestSetup
    {
        private List<int> _batchIds;
        private CertificatePrintingResponse _expectedResult;
        private BatchSummaryResponse _apiResponse;

        public override void Given()
        {
            _apiResponse = null;
            _batchIds = null;
            PrintingService.GetPendingPrintBatchesForBatchSummaryAsync().Returns(_batchIds);

            PrintingApiClient.GetBatchSummaryInfoAsync(Arg.Any<int>()).Returns(_apiResponse);

            _expectedResult = new CertificatePrintingResponse { IsSuccess = true, TotalCount = 0, PrintingProcessedCount = 0, ModifiedCount = 0, SavedCount = 0 };
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
            PrintingApiClient.DidNotReceive().GetBatchSummaryInfoAsync(Arg.Any<int>());
            PrintingService.DidNotReceive().UpdateBatchSummaryResponsesAsync(Arg.Any<List<BatchSummaryResponse>>());
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
