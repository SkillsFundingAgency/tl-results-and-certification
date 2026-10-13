using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressValidationReminderTests
{
    public abstract class TestSetup : ProviderAddressValidationReminderTestBase
    {
        public override async Task When()
        {
            await ProviderAddressValidationReminderFunction.ProviderAddressValidationReminderAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<ProviderAddressValidationReminder>());
        }
    }
}