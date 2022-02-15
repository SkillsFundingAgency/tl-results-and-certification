using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IResultRepository
    {
        Task<IEnumerable<TqRegistrationPathway>> GetBulkResultsAsync(long aoUkprn, IEnumerable<long> uniqueLearnerNumbers);
        Task<IList<TqPathwayResult>> GetBulkPathwayResultsAsync(IList<TqPathwayResult> pathwayResults);
        Task<bool> BulkInsertOrUpdateResults(List<TqPathwayResult> pathwayResults, List<TqSpecialismResult> specialismResults);
        Task<TqRegistrationPathway> GetResultsAsync(long aoUkprn, int profileId);
        Task<TqRegistrationPathway> GetPathwayResultAsync(long aoUkprn, int profileId, int assessmentId);
        Task<IList<TqSpecialismResult>> GetBulkSpecialismResultsAsync(IList<TqSpecialismResult> specialismResults);
    }
}