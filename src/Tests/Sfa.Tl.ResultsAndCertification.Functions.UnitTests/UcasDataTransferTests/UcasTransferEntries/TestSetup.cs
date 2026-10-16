using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests.UcasTransferEntries
{
    public abstract class TestSetup : UcasDataTransferTestBase
    {
        public async override Task When()
        {
            await UcasDataTransferFunction.UcasTransferEntriesAsync(new TimerInfo(), FunctionContext);
        }
    }
}
