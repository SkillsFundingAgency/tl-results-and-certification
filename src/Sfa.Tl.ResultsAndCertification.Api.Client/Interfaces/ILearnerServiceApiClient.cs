using Lrs.LearnerService.Api.Client;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface ILearnerServiceApiClient
    {
        Task<verifyLearnerResponse> VerifyLearnerAsync(RegisteredLearnerDetails learnerDetails);
        Task<findLearnerByULNResponse> FetchLearnerDetailsAsync(RegisteredLearnerDetails learnerDetails);
    }
}