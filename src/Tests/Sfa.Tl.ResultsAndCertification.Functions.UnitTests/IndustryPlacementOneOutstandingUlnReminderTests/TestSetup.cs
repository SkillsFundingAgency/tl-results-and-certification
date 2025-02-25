using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementOneOutstandingUlnReminderTests
{
    public abstract class TestSetup : IndustryPlacementOneOutstandingUlnReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementOneOutstandingUlnReminderFunction.IndustryPlacementOneOutstandingUlnReminderAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<IndustryPlacementOneOutstandingUlnReminder>());
        }
    }
}
