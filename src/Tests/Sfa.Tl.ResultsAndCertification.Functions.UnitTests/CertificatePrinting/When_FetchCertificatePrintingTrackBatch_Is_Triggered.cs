using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificatePrinting
{
    public class When_FetchCertificatePrintingTrackBatch_Is_Triggered : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            CertificatePrintingService.ProcessTrackBatchAsync().Returns(new CertificatePrintingResponse { IsSuccess = true });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }
        public async override Task When()
        {
            await CertificatePrintingFunction.FetchCertificatePrintingTrackBatchAsync(new TimerInfo(), FunctionContext);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            CertificatePrintingService.Received(1).ProcessTrackBatchAsync();
            CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}
