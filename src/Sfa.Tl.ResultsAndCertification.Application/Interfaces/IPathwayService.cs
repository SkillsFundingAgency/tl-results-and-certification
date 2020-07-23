using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces
{
    public interface IPathwayService
    {
        Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long aoUkprn, int pathwayId);
        Task<PathwaySpecialisms> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId);
    }
}