using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.AnalystCoreResultExtractionService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.AnalystCoreResultExtractionService
{
    public abstract class TestSetup : AnalystCoreResultExtractFunctionTestBase
    {
        public async override Task When()
        {
            await AnalystCoreResultExtractionFunction.AnalystCoreResultExtractionServiceAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<AnalystCoreResultExtraction>());
        }
    }
}
