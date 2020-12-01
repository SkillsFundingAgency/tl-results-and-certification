using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAssessmentLoader
    {
        Task<UploadAssessmentsResponseViewModel> ProcessBulkAssessmentsAsync(UploadAssessmentsRequestViewModel viewModel);
        Task<Stream> GetAssessmentValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference);
        Task<UlnAssessmentsNotFoundViewModel> FindUlnAssessmentsAsync(long aoUkprn, long Uln);
        Task<AssessmentDetailsViewModel> GetAssessmentDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
        Task<AddAssessmentSeriesViewModel> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, AssessmentEntryType assessmentEntryType);
        Task<AddAssessmentSeriesResponse> AddAssessmentSeriesAsync(AddAssessmentSeriesRequest request);
        Task<AssessmentEntryDetailsViewModel> GetActiveAssessmentEntryDetailsAsync(long aoUkprn, int assessmentId, AssessmentEntryType assessmentEntryType);
    }
}
