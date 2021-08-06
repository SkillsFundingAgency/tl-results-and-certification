using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificatePrintingService
{
    public class When_Called_ProcessTrackBatch_With_No_Data : TestSetup
    {
        private List<int> _batchIds;
        private CertificatePrintingResponse _expectedResult;
        private TrackBatchResponse _apiResponse;

        public override void Given()
        {
            _apiResponse = null;
            _batchIds = null;
            PrintingService.GetPendingItemsForTrackBatchAsync().Returns(_batchIds);

            PrintingApiClient.GetTrackBatchInfoAsync(Arg.Any<int>()).Returns(_apiResponse);

            _expectedResult = new CertificatePrintingResponse { IsSuccess = true, TotalCount = 0, PrintingProcessedCount = 0, ModifiedCount = 0, SavedCount = 0 };
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
            PrintingApiClient.DidNotReceive().GetTrackBatchInfoAsync(Arg.Any<int>());
            PrintingService.DidNotReceive().UpdateTrackBatchResponsesAsync(Arg.Any<List<TrackBatchResponse>>());
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
