using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminAssessmentSeriesDatesService
    {
        Task<GetAssessmentSeriesDatesDetailsResponse> GetAssessmentSeriesDateAsync(int assessmentId);
        Task<IEnumerable<GetAssessmentSeriesDatesDetailsResponse>> SearchAssessmentSeriesDatesAsync(SearchAssessmentSeriesDatesRequest request);
    }
}