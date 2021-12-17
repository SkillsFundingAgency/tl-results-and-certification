using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces
{
    public interface IDataExportLoader
    {
        Task<DataExportResponse> ProcessDataExportAsync(long aoUkprn, DataExportType requestType, string requestedBy);
    }
}
