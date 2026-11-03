using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementMissedDeadlineReminderTests
{
    public abstract class TestSetup : IndustryPlacementMissedDeadlineReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementMissedDeadlineReminderFunction.IndustryPlacementMissedDeadlineReminderAsync(new TimerInfo { ScheduleStatus = new ScheduleStatus() }, FunctionContextBuilder.Create());
        }
    }
}
