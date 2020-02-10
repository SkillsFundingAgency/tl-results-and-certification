using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IResultsAndCertificationInternalApiClient
    {
        Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByAwardingOrganisationAsync();
    }
}
