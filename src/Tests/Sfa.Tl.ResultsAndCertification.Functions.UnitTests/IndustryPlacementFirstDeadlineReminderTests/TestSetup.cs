using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementFirstDeadlineReminderTests
{
    public abstract class TestSetup : IndustryPlacementFirstDeadlineReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementFirstDeadlineReminderFunction.IndustryPlacementFirstDeadlineReminderAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<IndustryPlacementFirstDeadlineReminder>());
        }
    }
}
