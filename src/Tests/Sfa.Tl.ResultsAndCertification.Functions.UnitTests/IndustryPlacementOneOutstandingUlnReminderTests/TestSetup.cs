using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementOneOutstandingUlnReminderTests
{
    public abstract class TestSetup : IndustryPlacementOneOutstandingUlnReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementOneOutstandingUlnReminderFunction.IndustryPlacementOneOutstandingUlnReminderAsync(new TimerInfo { ScheduleStatus = new ScheduleStatus() }, FunctionContextBuilder.Create());
        }
    }
}
