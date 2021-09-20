using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface ICommonRepository
    {
        Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn);
        Task<IEnumerable<AcademicYear>> GetCurrentAcademicYearsAsync();
    }
}
