using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface ILearnerServiceApiClient
    {
        Task<bool> VerifyLearnerAsync(string uln, string firstName, string lastName, string dateOfBirth);
    }
}
