using System.Threading.Tasks;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface ITlevelLoader
    {
        Task<IEnumerable<YourTlevelsViewModel>> GetAllTlevelsByUkprnAsync(long ukprn);
        Task<TLevelDetailsViewModel> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id);
        Task<ConfirmTlevelViewModel> GetVerifyTlevelDetailsByPathwayIdAsync(long ukprn, int id);
        Task<SelectToReviewPageViewModel> GetTlevelsToReviewByUkprnAsync(long ukprn);
        Task<IEnumerable<YourTlevelsViewModel>> GetTlevelsByStatusIdAsync(long ukprn, int statusId);
        Task<bool> ConfirmTlevelAsync(ConfirmTlevelViewModel viewModel);
        Task<bool> ReportIssueAsync(TlevelQueryViewModel viewModel);
        Task<TlevelConfirmationViewModel> GetTlevelConfirmationDetailsAsync(long ukprn, int pathwayId);
        Task<TlevelQueryViewModel> GetQueryTlevelViewModelAsync(long ukprn, int id);
    }
}
