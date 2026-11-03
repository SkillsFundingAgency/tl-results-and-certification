using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementFirstDeadlineReminderTests
{
    public abstract class TestSetup : IndustryPlacementFirstDeadlineReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementFirstDeadlineReminderFunction.IndustryPlacementFirstDeadlineReminderAsync(new TimerInfo { ScheduleStatus = new ScheduleStatus() }, FunctionContextBuilder.Create());
        }
    }
}
