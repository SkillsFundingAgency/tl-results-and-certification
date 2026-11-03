using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressMissingReminderTests
{
    public abstract class TestSetup : ProviderAddressMissingReminderTestBase
    {
        public override async Task When()
        {
            await ProviderAddressMissingReminderFunction.ProviderAddressMissingReminderAsync(new TimerInfo { ScheduleStatus = new ScheduleStatus() }, FunctionContextBuilder.Create());
        }
    }
}