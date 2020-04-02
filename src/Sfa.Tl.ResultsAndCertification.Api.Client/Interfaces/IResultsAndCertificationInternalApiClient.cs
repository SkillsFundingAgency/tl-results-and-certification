using Sfa.Tl.ResultsAndCertification.Models.Contracts;
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
        Task<ProviderTlevelDetails> GetTqProviderTlevelDetailsAsync(int id);
    }
}
