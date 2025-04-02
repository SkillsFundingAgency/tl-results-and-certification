using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IAssessmentSeriesRepository
    {
        Task<AssessmentSeries> GetPreviousAssessmentSeriesAsync(DateTime utcNow);
    }
}