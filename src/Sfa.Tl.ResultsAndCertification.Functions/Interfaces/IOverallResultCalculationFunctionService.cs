using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Interfaces
{
    public interface IOverallResultCalculationFunctionService
    {
        Task<IList<OverallResultResponse>> CalculateOverallResultsAsync();
    }
}
