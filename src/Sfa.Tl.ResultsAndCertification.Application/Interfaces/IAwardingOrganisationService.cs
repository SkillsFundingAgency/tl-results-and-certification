using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAwardingOrganisationService
    {
        Task<IEnumerable<AwardingOrganisationMetadata>> GetAllAwardingOrganisationsAsync();

        Task<AwardingOrganisationMetadata> GetAwardingOrganisationByUkprnAsync(long ukprn);
    }
}