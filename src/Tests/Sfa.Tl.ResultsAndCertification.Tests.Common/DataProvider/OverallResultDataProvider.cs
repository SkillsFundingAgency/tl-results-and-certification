﻿using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class OverallResultDataProvider
    {
        public static OverallResult CreateOverallResult(ResultsAndCertificationDbContext _dbContext, TqRegistrationPathway tqRegistrationPathway, bool addToDbContext = true)
        {
            var overallResult = new OverallResultBuilder().Build(tqRegistrationPathway);

            if (addToDbContext)
            {
                _dbContext.Add(overallResult);
            }
            return overallResult;
        }

        public static OverallResult CreateOverallResult(ResultsAndCertificationDbContext _dbContext, TqRegistrationPathway tqRegistrationPathway, OverallResult overallResult, bool addToDbContext = true)
        {
            if (overallResult == null)
            {
                overallResult = new OverallResultBuilder().Build(tqRegistrationPathway);
            }

            if (addToDbContext)
            {
                _dbContext.Add(overallResult);
            }
            return overallResult;
        }

        public static IList<OverallResult> CreateOverallResult(ResultsAndCertificationDbContext _dbContext, IList<OverallResult> overallResults, bool addToDbContext = true)
        {
            if (addToDbContext && overallResults != null && overallResults.Count > 0)
            {
                _dbContext.AddRange(overallResults);
            }
            return overallResults;
        }
    }
}
