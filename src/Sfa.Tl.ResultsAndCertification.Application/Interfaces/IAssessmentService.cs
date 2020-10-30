using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAssessmentService
    {
        Task<bool> CompareAndProcessAssessmentsAsync(IList<TqPathwayAssessment> pathwayAssessments, IList<TqSpecialismAssessment> specialismAssessments);
    }
}
