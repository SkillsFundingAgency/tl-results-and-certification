using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces
{
    public interface IDataExportLoader
    {
        Task<IList<DataExportResponse>> ProcessDataExportAsync(long aoUkprn, DataExportType requestType, string requestedBy);
        Task<DataExportResponse> DownloadOverallResultsDataAsync(long providerUkprn, string requestedBy);
        Task<DataExportResponse> DownloadOverallResultSlipsDataAsync(long providerUkprn, string requestedBy);
        Task<DataExportResponse> DownloadLearnerOverallResultSlipsDataAsync(long providerUkprn, long profileId, string requestedBy);
    }
}
