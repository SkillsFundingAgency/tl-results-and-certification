using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressMissingReminderTests
{
    public abstract class TestSetup : ProviderAddressMissingReminderTestBase
    {
        public override async Task When()
        {
            await ProviderAddressMissingReminderFunction.ProviderAddressMissingReminderAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<ProviderAddressMissingReminder>());
        }
    }
}