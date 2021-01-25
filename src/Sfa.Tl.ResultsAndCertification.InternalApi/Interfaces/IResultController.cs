using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IResultController
    {
        Task<BulkResultResponse> ProcessBulkResultsAsync(BulkProcessRequest request);
        Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
        Task<AddResultResponse> AddResultAsync(AddResultRequest request);
        Task<CoreResult> GetCoreResultAsync(long aoUkprn, int profileId, int assessmentId);
        Task<IEnumerable<LookupData>> GetLookupDataAsync(int lookupCategory);
    }
}
