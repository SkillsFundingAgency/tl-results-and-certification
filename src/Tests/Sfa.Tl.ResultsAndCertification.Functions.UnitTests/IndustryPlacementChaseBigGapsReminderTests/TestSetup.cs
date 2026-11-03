using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementChaseBigGapsReminderTests
{
    public abstract class TestSetup : IndustryPlacementChaseBigGapsReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementChaseBigGapsReminderFunction.IndustryPlacementChaseBigGapsReminderAsync(new TimerInfo { ScheduleStatus = new ScheduleStatus() }, FunctionContextBuilder.Create());
        }
    }
}
