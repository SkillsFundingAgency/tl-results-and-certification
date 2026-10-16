using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementFirstDeadlineReminderTests
{
    public abstract class TestSetup : IndustryPlacementFirstDeadlineReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementFirstDeadlineReminderFunction.IndustryPlacementFirstDeadlineReminderAsync(new TimerInfo(), FunctionContext);
        }
    }
}
