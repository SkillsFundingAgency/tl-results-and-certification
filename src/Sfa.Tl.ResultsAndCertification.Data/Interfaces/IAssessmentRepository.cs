using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IAssessmentRepository : IRepository<TqPathwayAssessment>
    {
        Task<IEnumerable<TqRegistrationPathway>> GetBulkAssessmentsAsync(long aoUkprn, IEnumerable<long> uniqueLearnerNumbers);
        Task<IList<TqPathwayAssessment>> GetBulkPathwayAssessmentsAsync(IList<TqPathwayAssessment> pathwayAssessments);
        Task<IList<TqSpecialismAssessment>> GetBulkSpecialismAssessmentsAsync(IList<TqSpecialismAssessment> specialismAssessments);
        Task<bool> BulkInsertOrUpdateAssessments(List<TqPathwayAssessment> pathwayAssessmentEntities, List<TqSpecialismAssessment> specialismAssessmentEntities);        
    }
}
