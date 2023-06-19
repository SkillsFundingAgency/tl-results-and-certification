using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class DualSpecialismOverallGradeLookupProvider
    {
        public static IList<DualSpecialismOverallGradeLookup> DualSpecialismOverallGradeLookupList(ResultsAndCertificationDbContext _dbContext, IList<TlLookup> tlLookups, bool addToDbContext = true)
        {
            IList<DualSpecialismOverallGradeLookup> dualSpecialismOverallGradeLookups = new DualSpecialismOverallGradeLookupBuilder().BuildList(tlLookups);

            if (addToDbContext)
            {
                _dbContext.AddRange(dualSpecialismOverallGradeLookups);
            }

            return dualSpecialismOverallGradeLookups;
        }
    }
}