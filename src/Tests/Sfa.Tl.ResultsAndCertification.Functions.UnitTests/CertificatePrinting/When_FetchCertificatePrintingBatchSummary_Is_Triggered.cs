using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificatePrinting
{
    public class When_FetchCertificatePrintingBatchSummary_Is_Triggered : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            CertificatePrintingService.ProcessBatchSummaryAsync().Returns(new CertificatePrintingResponse { IsSuccess = true });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        public async override Task When()
        {
            await CertificatePrintingFunction.FetchCertificatePrintingBatchSummaryAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<Functions.CertificatePrinting>());
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            CertificatePrintingService.Received(1).ProcessBatchSummaryAsync();
            CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}
