using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminAssessmentSeriesDatesLoader
    {
        AdminAssessmentSeriesDatesCriteriaViewModel LoadFilters();

        //Task<List<AdminAssessmentSeriesDateDetailsViewModel>> SearchAssessmentSeriesDatesAsync(AdminAssessmentSeriesDatesCriteriaViewModel criteria);

        Task<List<AdminAssessmentSeriesDateDetailsViewModel>> GetAssessmentSeriesDatesAsync();

        Task<AdminAssessmentSeriesDateDetailsViewModel> GetAssessmentSeriesDatesDetailsViewModel(int assessmentId);
    }
}