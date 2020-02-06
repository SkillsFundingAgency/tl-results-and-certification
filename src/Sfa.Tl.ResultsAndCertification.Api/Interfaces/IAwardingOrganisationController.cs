using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IAwardingOrganisationController
    {
        Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByAwardingOrganisationIdAsync(int id);
    }
}