using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class TqSpecialismResultDataProvider
    {
        public static TqSpecialismResult CreateTqSpecialismResult(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var specialismResult = new TqSpecialismResultBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(specialismResult);
            }
            return specialismResult;
        }

        public static TqSpecialismResult CreateTqSpecialismResult(ResultsAndCertificationDbContext _dbContext, TqSpecialismResult specialismResult, bool addToDbContext = true)
        {
            if (specialismResult == null)
            {
                specialismResult = new TqSpecialismResultBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(specialismResult);
            }
            return specialismResult;
        }

        public static TqSpecialismResult CreateTqSpecialismResult(ResultsAndCertificationDbContext _dbContext, int TqSpecialismAssessmentId, int tlLookupId, DateTime startDate, PrsStatus? prsStatus, bool addToDbContext = true)
        {
            var specialismResult = new TqSpecialismResult
            {
                TqSpecialismAssessmentId = TqSpecialismAssessmentId,
                TlLookupId = tlLookupId,
                StartDate = startDate,
                PrsStatus = prsStatus,
                IsOptedin = true,
                IsBulkUpload = false
            };

            if (addToDbContext)
            {
                _dbContext.Add(specialismResult);
            }
            return specialismResult;
        }

        public static List<TqSpecialismResult> CreateTqSpecialismResults(ResultsAndCertificationDbContext _dbContext, List<TqSpecialismResult> specialismResults, bool addToDbContext = true)
        {
            if (addToDbContext && specialismResults != null && specialismResults.Count > 0)
            {
                _dbContext.AddRange(specialismResults);
            }
            return specialismResults;
        }
    }
}
