using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasEntries
{
    public abstract class TestBase : UcasDataTransferServiceTestBase
    {
        public async override Task When()
        {
            ActualResult = await Service.ProcessUcasEntriesAsync();
        }
    }
}