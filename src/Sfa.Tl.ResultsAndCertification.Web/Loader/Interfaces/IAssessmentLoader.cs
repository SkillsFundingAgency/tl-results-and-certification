using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
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
        Task<T> GetAssessmentDetailsAsync<T>(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
        Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(long aoUkprn, AddAssessmentEntryViewModel viewModel);
        Task<AssessmentEntryDetailsViewModel> GetActiveAssessmentEntryDetailsAsync(long aoUkprn, int assessmentId, ComponentType componentType);
        Task<bool> RemoveAssessmentEntryAsync(long aoUkprn, AssessmentEntryDetailsViewModel viewModel);        
        Task<T> GetAddAssessmentEntryAsync<T>(long aoUkprn, int profileId, ComponentType componentType, string componentLarIds);
        Task<AddAssessmentEntryResponse> AddSpecialismAssessmentEntryAsync(long aoUkprn, AddSpecialismAssessmentEntryViewModel viewModel);
        Task<RemoveSpecialismAssessmentEntryViewModel> GetRemoveSpecialismAssessmentEntriesAsync(long aoUkprn, int profileId, string specialismAssessmentIds);
        Task<bool> RemoveSpecialismAssessmentEntryAsync(long aoUkprn, RemoveSpecialismAssessmentEntryViewModel viewModel);
    }
}
