using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IOverallResultCalculationService
    {
        Task<int> GetResultCalculationYearOfAsync(DateTime runDate);
        Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculationAsync(DateTime runDate);
        Task<bool> CalculateOverallResultsAsync(DateTime runDate);
    }
}
