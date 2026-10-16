using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementOneOutstandingUlnReminderTests
{
    public abstract class TestSetup : IndustryPlacementOneOutstandingUlnReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementOneOutstandingUlnReminderFunction.IndustryPlacementOneOutstandingUlnReminderAsync(new TimerInfo(), FunctionContext);
        }
    }
}
