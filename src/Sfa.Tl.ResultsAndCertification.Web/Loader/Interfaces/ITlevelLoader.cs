using System.Threading.Tasks;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.Models;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface ITlevelLoader
    {
        Task<IEnumerable<YourTlevelsViewModel>> GetAllTlevelsByUkprnAsync(long ukprn);
        Task<TLevelDetailsViewModel> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id);
        Task<IEnumerable<YourTlevelsViewModel>> GetTlevelsByStatusIdAsync(long ukprn, int statusId);
    }
}
