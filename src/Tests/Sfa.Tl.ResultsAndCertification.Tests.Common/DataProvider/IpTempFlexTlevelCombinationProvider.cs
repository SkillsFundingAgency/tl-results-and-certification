using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class IpTempFlexTlevelCombinationProvider
    {
        public static IpTempFlexTlevelCombination CreateIpTempFlexTlevelCombination(ResultsAndCertificationDbContext _dbContext, EnumAwardingOrganisation awardingOrganisation, bool addToDbContext = true)
        {
            var ipTempFlexTlevelCombination = new IpTempFlexTlevelCombinationBuilder().Build(awardingOrganisation);

            if (addToDbContext && ipTempFlexTlevelCombination != null)
            {
                _dbContext.Add(ipTempFlexTlevelCombination);
            }
            return ipTempFlexTlevelCombination;
        }

        public static IpTempFlexTlevelCombination CreateIpTempFlexTlevelCombination(ResultsAndCertificationDbContext _dbContext, IpTempFlexTlevelCombination ipTempFlexTlevelCombination, bool addToDbContext = true)
        {
            if (addToDbContext && ipTempFlexTlevelCombination == null)
            {
                _dbContext.Add(ipTempFlexTlevelCombination);
            }
            return ipTempFlexTlevelCombination;
        }

        public static IpTempFlexTlevelCombination CreateIpTempFlexTlevelCombination(ResultsAndCertificationDbContext _dbContext, TlPathway tlPathway, IpLookup ipLookup, bool addToDbContext = true)
        {
            if (tlPathway != null)
            {
                var ipTempFlexTlevelCombination = new IpTempFlexTlevelCombination
                {
                    TlPathwayId = tlPathway.Id,
                    IpLookupId = ipLookup.Id,
                    TlPathway = tlPathway,
                    IpLookup = ipLookup,
                    IsActive = true
                };

                if (addToDbContext)
                {
                    _dbContext.Add(ipTempFlexTlevelCombination);
                }
                return ipTempFlexTlevelCombination;
            }
            return null;
        }

        public static IList<IpTempFlexTlevelCombination> CreateIpTempFlexTlevelCombinationsList(ResultsAndCertificationDbContext _dbContext, EnumAwardingOrganisation awardingOrganisation, IList<IpTempFlexTlevelCombination> ipTempFlexTlevelCombinations = null, bool addToDbContext = true)
        {
            if (ipTempFlexTlevelCombinations == null)
                ipTempFlexTlevelCombinations = new IpTempFlexTlevelCombinationBuilder().BuildList(awardingOrganisation);

            if (addToDbContext)
            {
                _dbContext.AddRange(ipTempFlexTlevelCombinations);
            }
            return ipTempFlexTlevelCombinations;
        }
    }
}
