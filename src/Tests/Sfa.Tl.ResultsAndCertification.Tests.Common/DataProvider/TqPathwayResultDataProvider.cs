using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class TqPathwayResultDataProvider
    {
        public static TqPathwayResult CreateTqPathwayResult(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var pathwayResult = new TqPathwayResultBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(pathwayResult);
            }
            return pathwayResult;
        }

        public static TqPathwayResult CreateTqPathwayResult(ResultsAndCertificationDbContext _dbContext, TqPathwayResult pathwayResult, bool addToDbContext = true)
        {
            if (pathwayResult == null)
            {
                pathwayResult = new TqPathwayResultBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(pathwayResult);
            }
            return pathwayResult;
        }

        public static TqPathwayResult CreateTqPathwayResult(ResultsAndCertificationDbContext _dbContext, int tqPathwayAssessmentId, int tlLookupId, DateTime startDate, PrsStatus? prsStatus, bool addToDbContext = true)
        {
            var pathwayResult = new TqPathwayResult
            {
                TqPathwayAssessmentId = tqPathwayAssessmentId,
                TlLookupId = tlLookupId,
                StartDate = startDate,
                PrsStatus = prsStatus,
                IsOptedin = true,
                IsBulkUpload = false
            };

            if (addToDbContext)
            {
                _dbContext.Add(pathwayResult);
            }
            return pathwayResult;
        }

        public static List<TqPathwayResult> CreateTqPathwayResults(ResultsAndCertificationDbContext _dbContext, List<TqPathwayResult> pathwayResults, bool addToDbContext = true)
        {
            if (addToDbContext && pathwayResults != null && pathwayResults.Count > 0)
            {
                _dbContext.AddRange(pathwayResults);
            }
            return pathwayResults;
        }
    }
}
