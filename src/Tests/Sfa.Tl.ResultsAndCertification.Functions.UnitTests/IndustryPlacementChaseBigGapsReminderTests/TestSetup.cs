using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementChaseBigGapsReminderTests
{
    public abstract class TestSetup : IndustryPlacementChaseBigGapsReminderTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementChaseBigGapsReminderFunction.IndustryPlacementChaseBigGapsReminderAsync(new TimerInfo(), FunctionContextFactory.Create());
        }
    }
}
