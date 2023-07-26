using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementExtractTests
{
    public abstract class TestSetup : IndustryPlacementExtractFunctionTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementExtractFunction.IndustryPlacementExtractAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<IndustryPlacementExtract>());
        }
    }
}
