using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasDataRecords
{
    public abstract class TestBase : UcasDataTransferServiceTestBase
    {
        protected UcasDataType UcasDataType { get; set; }

        public async override Task When()
        {
            ActualResult = await Service.ProcessUcasDataRecordsAsync(UcasDataType);
        }
    }
}