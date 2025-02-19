using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementMissedDeadlineReminderTests
{
    public abstract class TestSetup : IndustryPlacementMissedDeadlineReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementMissedDeadlineReminderFunction.IndustryPlacementMissedDeadlineReminderAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<IndustryPlacementFirstDeadlineReminder>());
        }
    }
}
