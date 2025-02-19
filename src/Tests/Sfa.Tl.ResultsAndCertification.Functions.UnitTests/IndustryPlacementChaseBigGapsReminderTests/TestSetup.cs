using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementChaseBigGapsReminderTests
{
    public abstract class TestSetup : IndustryPlacementChaseBigGapsReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementChaseBigGapsReminderFunction.IndustryPlacementChaseBigGapsReminderAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<IndustryPlacementChaseBigGapsReminder>());
        }
    }
}
