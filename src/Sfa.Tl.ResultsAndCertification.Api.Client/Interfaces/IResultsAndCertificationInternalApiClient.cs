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
        Task<IList<CoreDetails>> GetRegisteredProviderCoreDetailsAsync(long aoUkprn, long providerUkprn);

        // DocumentUploadHistory
        Task<DocumentUploadHistoryDetails> GetDocumentUploadHistoryDetailsAsync(long aoUkprn, Guid blobUniqueReference);
    }
}
