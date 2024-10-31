using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAssessmentSeriesLoader
    {
        public Task<AssessmentSeriesDetails> GetResultCalculationAssessmentAsync();
    }
}
