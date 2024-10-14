using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ICommonService
    {
        Task<IEnumerable<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory);

        Task<IEnumerable<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory, List<string> codoes);

        Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn);

        // FunctionLog
        Task<bool> CreateFunctionLog(FunctionLogDetails model);

        Task<bool> UpdateFunctionLog(FunctionLogDetails model);

        Task<bool> SendFunctionJobFailedNotification(string jobName, string errorMessage);

        Task<IEnumerable<AcademicYear>> GetCurrentAcademicYearsAsync();

        Task<IEnumerable<AcademicYear>> GetAcademicYearsAsync();

        Task<IEnumerable<Assessment>> GetAssessmentSeriesAsync();

        bool IsIndustryPlacementTriggerDateValid();

        DateTime CurrentDate { get; }
    }
}