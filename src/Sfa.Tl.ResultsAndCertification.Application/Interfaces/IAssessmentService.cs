using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAssessmentService
    {
        Task<bool> CompareAndProcessAssessmentsAsync(IList<TqPathwayAssessment> pathwayAssessments, IList<TqSpecialismAssessment> specialismAssessments);
        Task<IList<AssessmentCsvRecordResponse>> ValidateAssessmentsAsync(long aoUkprn, IEnumerable<AssessmentCsvRecordResponse> enumerable);
    }
}
