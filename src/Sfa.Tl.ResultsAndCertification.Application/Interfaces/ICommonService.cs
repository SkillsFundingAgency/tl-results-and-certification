using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ICommonService
    {
        Task<IEnumerable<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory);
        Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn);

        // FunctionLog
        Task<bool> CreateFunctionLog(FunctionLogDetails model);
        Task<bool> UpdateFunctionLog(FunctionLogDetails model);

        Task<bool> SendFunctionJobFailedNotification(string jobName, string errorMessage);
        Task<IEnumerable<AcademicYear>> GetCurrentAcademicYearsAsync();
    }
}
