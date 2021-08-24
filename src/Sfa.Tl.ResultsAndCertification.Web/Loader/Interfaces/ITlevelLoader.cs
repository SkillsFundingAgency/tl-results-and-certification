using System.Threading.Tasks;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface ITlevelLoader
    {
        Task<YourTlevelsViewModel> GetYourTlevelsViewModel(long ukprn);
        Task<TLevelDetailsViewModel> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id);
        Task<ConfirmTlevelViewModel> GetVerifyTlevelDetailsByPathwayIdAsync(long ukprn, int id);
        Task<SelectToReviewPageViewModel> GetTlevelsToReviewByUkprnAsync(long ukprn);
        Task<IEnumerable<YourTlevelViewModel>> GetTlevelsByStatusIdAsync(long ukprn, int statusId);
        Task<ConfirmedTlevelsViewModel> GetYourTlevelsByStatusAsync(long ukprn, int statusId);
        Task<bool> ConfirmTlevelAsync(ConfirmTlevelViewModel viewModel);
        Task<bool> ReportIssueAsync(TlevelQueryViewModel viewModel);
        Task<TlevelConfirmationViewModel> GetTlevelConfirmationDetailsAsync(long ukprn, int pathwayId);
        Task<TlevelQueryViewModel> GetQueryTlevelViewModelAsync(long ukprn, int id);
    }
}
