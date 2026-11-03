using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressValidationReminderTests
{
    public abstract class TestSetup : ProviderAddressValidationReminderTestBase
    {
        public override async Task When()
        {
            await ProviderAddressValidationReminderFunction.ProviderAddressValidationReminderAsync(new TimerInfo { ScheduleStatus = new ScheduleStatus() }, FunctionContextBuilder.Create());
        }
    }
}