using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IDataExportController
    {
        Task<IList<DataExportResponse>> GetDataExportAsync(long aoUkprn, DataExportType requestType, string requestedBy);
    }
}
