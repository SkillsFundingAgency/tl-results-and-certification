using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IUcasDataService
    {
        Task<UcasData> ProcessUcasDataRecordsAsync(UcasDataType ucasDataType);

        Task<UcasData> ProcessUcasDataRecordsTestAsync(UcasDataType ucasDataType);

    }
}
