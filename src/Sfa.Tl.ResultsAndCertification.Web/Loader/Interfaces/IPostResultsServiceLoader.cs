using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IPostResultsServiceLoader
    {
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null);
        Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId);
        Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId, int assessmentId, ComponentType componentType);
        Task<bool> PrsRommActivityAsync(long aoUkprn, PrsAddRommOutcomeViewModel model);
        Task<bool> PrsRommActivityAsync(long aoUkprn, PrsAddRommOutcomeKnownViewModel model);
        Task<bool> PrsRommActivityAsync(long aoUkprn, PrsRommCheckAndSubmitViewModel model);
        Task<bool> PrsAppealActivityAsync(long aoUkprn, PrsAddAppealOutcomeViewModel model);
        Task<bool> PrsAppealActivityAsync(long aoUkprn, PrsAddAppealOutcomeKnownViewModel model);
        Task<bool> PrsAppealActivityAsync(long aoUkprn, PrsAppealCheckAndSubmitViewModel model);
        Task<bool> PrsGradeChangeRequestAsync(PrsGradeChangeRequestViewModel model);
        T TransformLearnerDetailsTo<T>(FindPrsLearnerRecord prsLearnerRecord);
        Task<UploadRommsResponseViewModel> ProcessBulkRommsAsync(UploadRommsRequestViewModel viewModel);
        Task<Stream> GetRommValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference);
        Task<IList<DataExportResponse>> GenerateRommsDataExportAsync(long aoUkprn, string requestedBy);
        Task<Stream> GetRommsDataFileAsync(long aoUkprn, Guid blobUniqueReference);
    }
}