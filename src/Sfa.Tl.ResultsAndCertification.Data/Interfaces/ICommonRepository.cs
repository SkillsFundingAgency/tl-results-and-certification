using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface ICommonRepository
    {
        Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn);
        Task<IEnumerable<AcademicYear>> GetCurrentAcademicYearsAsync();
        Task<IEnumerable<AcademicYear>> GetAcademicYearsAsync();
        Task<IEnumerable<Assessment>> GetAssessmentSeriesAsync();
    }
}
