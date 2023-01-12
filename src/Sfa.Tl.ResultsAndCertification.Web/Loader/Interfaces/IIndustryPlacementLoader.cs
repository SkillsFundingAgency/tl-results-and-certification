using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IIndustryPlacementLoader
    {
        Task<UploadIndustryPlacementsResponseViewModel> ProcessBulkIndustryPlacementsAsync(UploadIndustryPlacementsRequestViewModel viewModel);
        Task<Stream> GetIndustryPlacementValidationErrorsFileAsync(long ukprn, Guid blobUniqueReference);
        Task<T> GetLearnerRecordDetailsAsync<T>(long providerUkprn, int profileId, int? pathwayId = null);
        Task<IndustryPlacementViewModel> GetIndustryPlacementViewModelAsync(long providerUkprn, int profileId); // TODO: rename
        Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId = null);
        Task<T> GetIpLookupDataAsync<T>(IpLookupType ipLookupType, string learnerName = null, int? pathwayId = null, bool showOption = false);
        Task<T> TransformIpCompletionDetailsTo<T>(IpCompletionViewModel model);
        Task<IList<IpLookupDataViewModel>> GetSpecialConsiderationReasonsListAsync(int academicYear);
        Task<bool> ProcessIndustryPlacementDetailsAsync(long providerUkprn, IndustryPlacementViewModel viewModel);
        (List<SummaryItemModel>, bool) GetIpSummaryDetailsListAsync(IndustryPlacementViewModel cacheModel);
        NotificationBannerModel GetSuccessNotificationBanner(IndustryPlacementStatus? industryPlacementStatus);
    }
}