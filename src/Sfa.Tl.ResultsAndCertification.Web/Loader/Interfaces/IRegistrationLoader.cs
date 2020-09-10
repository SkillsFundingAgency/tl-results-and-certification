using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
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
        Task<bool> AddRegistrationAsync(long aoUkprn, RegistrationViewModel model);
        Task<UlnNotFoundViewModel> FindUlnAsync(long aoUkprn, long uln);
        Task<RegistrationDetailsViewModel> GetRegistrationDetailsByProfileIdAsync(long aoUkprn, int profileId);
        Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId);

        // Change Registration loaders
        Task<T> GetRegistrationProfileAsync<T>(long aoUkprn, int profileId);
        Task<ManageRegistrationResponse> ProcessProfileNameChangeAsync(long aoUkprn, ChangeLearnersNameViewModel viewModel);
        Task<ManageRegistrationResponse> ProcessDateofBirthChangeAsync(long aoUkprn, ChangeDateofBirthViewModel viewModel);
        Task<ProviderChangeResponse> ProcessProviderChangesAsync(long aoUkprn, ChangeProviderViewModel viewModel);
        Task<ManageRegistrationResponse> ProcessSpecialismQuestionChangeAsync(long aoUkprn, ChangeSpecialismQuestionViewModel viewModel);
    }
}