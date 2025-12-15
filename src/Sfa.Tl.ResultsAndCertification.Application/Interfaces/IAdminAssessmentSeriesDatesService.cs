using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminAssessmentSeriesDatesService
    {
        Task<GetAssessmentSeriesDatesDetailsResponse> GetAssessmentSeriesDateAsync(int assessmentId);
        Task<PagedResponse<GetAssessmentSeriesDatesDetailsResponse>> SearchAssessmentSeriesDatesAsync(SearchAssessmentSeriesDatesRequest request);
    }
}