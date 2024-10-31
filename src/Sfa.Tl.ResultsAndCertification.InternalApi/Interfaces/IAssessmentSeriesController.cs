using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IAssessmentSeriesController
    {
        Task<AssessmentSeriesDetails> GetResultCalculationAssessmentAsync();
    }
}