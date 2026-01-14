using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminAssessmentSeriesDatesLoader
    {
        AdminAssessmentSeriesDatesCriteriaViewModel LoadFilters();

        Task<AdminAssessmentSeriesDatesViewModel> SearchAssessmentSeriesDatesAsync(AdminAssessmentSeriesDatesCriteriaViewModel criteria);

        Task<AdminAssessmentSeriesDetailsViewModel> GetAssessmentSeriesDateViewModel(int assessmentId);
    }
}