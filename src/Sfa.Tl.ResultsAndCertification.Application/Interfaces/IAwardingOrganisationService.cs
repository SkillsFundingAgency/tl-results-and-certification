using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces
{
    public interface IAwardingOrganisationService
    {
        Task<IEnumerable<string>> GetAllTlevelsByAwardingOrganisationIdAsync(int id);
    }
}