﻿using System.Threading.Tasks;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface ITlevelLoader
    {
        // Verify
        Task<SelectToReviewPageViewModel> GetTlevelsToReviewByUkprnAsync(long ukprn);
        Task<IEnumerable<YourTlevelViewModel>> GetTlevelsByStatusIdAsync(long ukprn, int statusId); 
        Task<ConfirmTlevelViewModel> GetVerifyTlevelDetailsByPathwayIdAsync(long ukprn, int id); 
        Task<bool> ConfirmTlevelAsync(ConfirmTlevelViewModel viewModel);
        Task<bool> ReportIssueAsync(TlevelQueryViewModel viewModel);

        // Confirmed
        Task<ConfirmedTlevelsViewModel> GetConfirmedTlevelsViewModelAsync(long ukprn);
        Task<TLevelConfirmedDetailsViewModel> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id);
        Task<TlevelConfirmationViewModel> GetTlevelConfirmationDetailsAsync(long ukprn, int pathwayId);

        // Queried
        Task<TlevelQueryViewModel> GetQueryTlevelViewModelAsync(long ukprn, int id); 
        Task<QueriedTlevelsViewModel> GetQueriedTlevelsViewModelAsync(long ukprn); 
        Task<TlevelQueriedDetailsViewModel> GetQueriedTlevelDetailsAsync(long ukprn, int id);
    }
}
