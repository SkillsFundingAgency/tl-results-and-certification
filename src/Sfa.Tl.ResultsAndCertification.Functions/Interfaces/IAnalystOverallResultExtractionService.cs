using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Interfaces
{
    public interface IAnalystOverallResultExtractionService
    {
        Task<FunctionResponse> ProcessAnalystOverallResultExtractionData(int[] academicYears);
    }
}