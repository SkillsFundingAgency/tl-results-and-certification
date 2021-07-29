using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificatePrintingService
{
    public class When_Called_ProcessPrintingRequest_With_Empty_Data : TestSetup
    {
        private List<PrintRequest> _printRequests;
        private CertificatePrintingResponse _expectedResult;
        private PrintResponse _apiResponse;

        public override void Given()
        {
            _apiResponse = null;
            _printRequests = new List<PrintRequest>();
            PrintingService.GetPendingPrintRequestsAsync().Returns(_printRequests);

            PrintingApiClient.ProcessPrintRequestAsync(Arg.Any<PrintRequest>()).Returns(_apiResponse);

            _expectedResult = new CertificatePrintingResponse { IsSuccess = true, TotalCount = 0, PrintingProcessedCount = 0, ModifiedCount = 0, SavedCount = 0 };
            PrintingService.UpdatePrintReqeustResponsesAsync(Arg.Any<List<PrintRequestResponse>>()).Returns(_expectedResult);
        }

        public async override Task When()
        {
            ActualResult = await Service.ProcessPrintingRequestAsync();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            PrintingService.Received(1).GetPendingPrintRequestsAsync();
            PrintingApiClient.DidNotReceive().ProcessPrintRequestAsync(Arg.Any<PrintRequest>());
            PrintingService.DidNotReceive().UpdatePrintReqeustResponsesAsync(Arg.Any<List<PrintRequestResponse>>());
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
