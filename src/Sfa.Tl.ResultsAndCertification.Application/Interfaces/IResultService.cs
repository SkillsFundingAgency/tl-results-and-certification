using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IResultService
    {
        Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
    }
}
