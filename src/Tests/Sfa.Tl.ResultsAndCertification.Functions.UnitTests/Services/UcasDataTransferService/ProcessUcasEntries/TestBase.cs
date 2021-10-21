using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasEntries
{
    public abstract class TestBase : UcasDataTransferServiceTestBase
    {
        protected UcasDataType UcasDataType { get; set; }

        public async override Task When()
        {
            ActualResult = await Service.ProcessUcasEntriesAsync(UcasDataType);
        }
    }
}