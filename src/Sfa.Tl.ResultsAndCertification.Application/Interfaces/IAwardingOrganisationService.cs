using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces
{
    public interface IAwardingOrganisationService
    {
        Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn);
        Task<IEnumerable<AwardingOrganisationPathwayReviewStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId);
    }
}