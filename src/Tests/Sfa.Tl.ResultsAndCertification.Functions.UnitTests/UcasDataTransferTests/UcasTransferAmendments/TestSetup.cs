using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests.UcasTransferAmendments
{
    public abstract class TestSetup : UcasDataTransferTestBase
    {
        public async override Task When()
        {
            await UcasDataTransferFunction.UcasTransferAmendmentsAsync(new TimerInfo { ScheduleStatus = new ScheduleStatus() }, FunctionContextBuilder.Create());
        }
    }
}
