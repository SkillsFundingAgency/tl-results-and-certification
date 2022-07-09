using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IOverallResultCalculationService
    {
        Task<AssessmentSeries> GetResultCalculationAssessmentAsync(DateTime runDate);
        Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculationAsync(DateTime runDate);
        Task<List<OverallResultResponse>> CalculateOverallResultsAsync(DateTime runDate);
    }
}