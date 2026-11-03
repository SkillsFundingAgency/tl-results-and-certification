using Microsoft.Azure.Functions.Worker;
using Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementExtractTests
{
    public abstract class TestSetup : IndustryPlacementExtractFunctionTestBase
    {
        public async override Task When()
        {
            await IndustryPlacementExtractFunction.IndustryPlacementExtractAsync(new TimerInfo { ScheduleStatus = new ScheduleStatus() }, FunctionContextBuilder.Create());
        }
    }
}
