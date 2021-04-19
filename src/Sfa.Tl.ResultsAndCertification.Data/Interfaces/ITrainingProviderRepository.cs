using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface ITrainingProviderRepository
    {
        Task<bool> IsSendConfirmationRequiredAsync(int profileId);
    }
}
