using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Helpers;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementMissedDeadlineReminderTests
{
    public abstract class TestSetup : IndustryPlacementMissedDeadlineReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementMissedDeadlineReminderFunction.IndustryPlacementMissedDeadlineReminderAsync(new TimerInfo(), FunctionContextFactory.Create());
        }
    }
}
