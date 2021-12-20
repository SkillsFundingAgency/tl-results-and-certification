using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface ILearnerRepository
    {
        Task<TqRegistrationPathway> GetLearnerRecordAsync(long aoUkprn, int profileId);
    }
}
