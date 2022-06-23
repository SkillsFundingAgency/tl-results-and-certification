using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class OverallGradeLookupProvider
    {
        public static OverallGradeLookup CreateOverallGradeLookup(ResultsAndCertificationDbContext _dbContext, TlPathway tlPathway, bool addToDbContext = true)
        {
            var overallGradeLookup = new OverallGradeLookupBuilder().Build(tlPathway);

            if (addToDbContext)
            {
                _dbContext.Add(overallGradeLookup);
            }
            return overallGradeLookup;
        }

        public static OverallGradeLookup CreateOverallGradeLookup(ResultsAndCertificationDbContext _dbContext, TlPathway tlPathway, OverallGradeLookup overallGradeLookup, bool addToDbContext = true)
        {
            if (overallGradeLookup == null)
            {
                overallGradeLookup = new OverallGradeLookupBuilder().Build(tlPathway);
            }

            if (addToDbContext)
            {
                _dbContext.Add(overallGradeLookup);
            }
            return overallGradeLookup;
        }

        public static IList<OverallGradeLookup> CreateOverallGradeLookup(ResultsAndCertificationDbContext _dbContext, IList<OverallGradeLookup> overallGradeLookups, bool addToDbContext = true)
        {
            if (addToDbContext && overallGradeLookups != null && overallGradeLookups.Count > 0)
            {
                _dbContext.AddRange(overallGradeLookups);
            }
            return overallGradeLookups;
        }
    }
}
