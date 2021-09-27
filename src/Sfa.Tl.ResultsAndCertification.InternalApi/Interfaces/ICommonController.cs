using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface ICommonController
    {
        Task<IEnumerable<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory);
        Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn);
        Task<IEnumerable<AcademicYear>> GetCurrentAcademicYearsAsync();
        Task<IEnumerable<AcademicYear>> GetAcademicYearsAsync();
    }
}