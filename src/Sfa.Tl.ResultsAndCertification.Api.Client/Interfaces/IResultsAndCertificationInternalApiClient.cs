using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IResultsAndCertificationInternalApiClient
    {
        // Tlevels
        Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn);
        Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id);
        Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId);
        Task<bool> VerifyTlevelAsync(VerifyTlevelDetails model);
        Task<PathwaySpecialisms> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId);

        // Providers
        Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn);
        Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch);
        Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId);
        Task<bool> AddProviderTlevelsAsync(IList<ProviderTlevel> model);
        Task<ProviderTlevels> GetAllProviderTlevelsAsync(long aoUkprn, int providerId);
        Task<IList<ProviderDetails>> GetTqAoProviderDetailsAsync(long aoUkprn);
        Task<ProviderTlevelDetails> GetTqProviderTlevelDetailsAsync(long aoUkprn, int tqProviderId);
        Task<bool> RemoveTqProviderTlevelAsync(long aoUkprn, int tqProviderId);
        Task<bool> HasAnyTlevelSetupForProviderAsync(long aoUkprn, int tlProviderId);

        //Registrations
        Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest model);
        Task<IList<PathwayDetails>> GetRegisteredProviderPathwayDetailsAsync(long aoUkprn, long providerUkprn);
        Task<bool> AddRegistrationAsync(RegistrationRequest model);
        Task<FindUlnResponse> FindUlnAsync(long aoUkprn, long uln);
        Task<RegistrationDetails> GetRegistrationDetailsAsync(long aoUkprn, int profileId);
        Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId);

        // Manage registrations
        Task<ManageRegistration> GetRegistrationAsync(long aoUkprn, int profileId);
        Task<bool> UpdateRegistrationAsync(ManageRegistration model);
        Task<bool> WithdrawRegistrationAsync(WithdrawRegistrationRequest model);
        Task<bool> ReJoinRegistrationAsync(ReJoinRegistrationRequest model);

        // DocumentUploadHistory
        Task<DocumentUploadHistoryDetails> GetDocumentUploadHistoryDetailsAsync(long aoUkprn, Guid blobUniqueReference);
    }
}
