using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IRegistrationLoader
    {
        Task<UploadRegistrationsResponseViewModel> ProcessBulkRegistrationsAsync(UploadRegistrationsRequestViewModel viewModel);
        Task<Stream> GetRegistrationValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference);
        Task<SelectProviderViewModel> GetRegisteredTqAoProviderDetailsAsync(long aoUkprn);
        Task<SelectCoreViewModel> GetRegisteredProviderPathwayDetailsAsync(long aoUkprn, long providerUkprn);
        Task<PathwaySpecialismsViewModel> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId);
        
        Task<UlnRegistrationNotFoundViewModel> FindUlnAsync(long aoUkprn, long uln);
        Task<RegistrationDetailsViewModel> GetRegistrationDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
        Task<RegistrationAssessmentDetails> GetRegistrationAssessmentAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null); 
        Task<bool> AddRegistrationAsync(long aoUkprn, RegistrationViewModel model);
        Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId);

        // Change Registration loaders
        Task<T> GetRegistrationProfileAsync<T>(long aoUkprn, int profileId, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active);
        Task<ManageRegistrationResponse> ProcessProfileNameChangeAsync(long aoUkprn, ChangeLearnersNameViewModel viewModel);
        Task<ManageRegistrationResponse> ProcessDateofBirthChangeAsync(long aoUkprn, ChangeDateofBirthViewModel viewModel);
        Task<ProviderChangeResponse> ProcessProviderChangesAsync(long aoUkprn, ChangeProviderViewModel viewModel);
        Task<ManageRegistrationResponse> ProcessSpecialismQuestionChangeAsync(long aoUkprn, ChangeSpecialismQuestionViewModel viewModel);
        Task<ManageRegistrationResponse> ProcessSpecialismChangeAsync(long aoUkprn, ChangeSpecialismViewModel viewModel);
        Task<ChangeCoreQuestionViewModel> GetRegistrationChangeCoreQuestionDetailsAsync(long aoUkprn, int profileId);
        Task<WithdrawRegistrationResponse> WithdrawRegistrationAsync(long aoUkprn, WithdrawRegistrationViewModel viewModel);
        Task<RejoinRegistrationResponse> RejoinRegistrationAsync(long aoUkprn, RejoinRegistrationViewModel viewModel);
        Task<ReregistrationResponse> ReregistrationAsync(long aoUkprn, ReregisterViewModel viewModel);

        Task<IEnumerable<AcademicYear>> GetCurrentAcademicYearsAsync();
        Task<IEnumerable<AcademicYear>> GetAcademicYearsAsync();
    }
}