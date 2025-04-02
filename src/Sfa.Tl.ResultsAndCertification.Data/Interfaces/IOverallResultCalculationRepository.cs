using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IOverallResultCalculationRepository
    {
        Task<IList<TqRegistrationPathway>> GetRegistrationsForOverallGradeCalculation(AssessmentSeries assessmentSeries);
    }
}
