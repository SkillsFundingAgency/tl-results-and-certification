using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Interfaces
{
    public interface ILearnerService
    {
        Task<LearnerGenderResponse> FetchLearnerGenderAsync();
    }
}
