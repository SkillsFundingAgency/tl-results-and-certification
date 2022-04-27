using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class IpModelTlevelCombinationProvider
    {
        public static IpModelTlevelCombination CreateIpModelTlevelCombination(ResultsAndCertificationDbContext _dbContext, EnumAwardingOrganisation awardingOrganisation, bool addToDbContext = true)
        {
            var ipModelTlevelCombination = new IpModelTlevelCombinationBuilder().Build(awardingOrganisation);

            if (addToDbContext && ipModelTlevelCombination != null)
            {
                _dbContext.Add(ipModelTlevelCombination);
            }
            return ipModelTlevelCombination;
        }

        public static IpModelTlevelCombination CreateIpModelTlevelCombination(ResultsAndCertificationDbContext _dbContext, IpModelTlevelCombination ipModelTlevelCombination, bool addToDbContext = true)
        {
            if (addToDbContext && ipModelTlevelCombination == null)
            {
                _dbContext.Add(ipModelTlevelCombination);
            }
            return ipModelTlevelCombination;
        }

        public static IpModelTlevelCombination CreateIpModelTlevelCombination(ResultsAndCertificationDbContext _dbContext, TlPathway tlPathway, IpLookup ipLookup, int groupId, bool addToDbContext = true)
        {
            if (tlPathway != null)
            {
                var ipModelTlevelCombination = new IpModelTlevelCombination
                {
                    TlPathwayId = tlPathway.Id,
                    IpLookupId = ipLookup.Id,
                    TlPathway = tlPathway,
                    IpLookup = ipLookup,
                    IsActive = true
                };

                if (addToDbContext)
                {
                    _dbContext.Add(ipModelTlevelCombination);
                }
                return ipModelTlevelCombination;
            }
            return null;
        }

        public static IList<IpModelTlevelCombination> CreateIpModelTlevelCombinationsList(ResultsAndCertificationDbContext _dbContext, EnumAwardingOrganisation awardingOrganisation, IList<IpModelTlevelCombination> ipModelTlevelCombinations = null, bool addToDbContext = true)
        {
            if (ipModelTlevelCombinations == null)
                ipModelTlevelCombinations = new IpModelTlevelCombinationBuilder().BuildList(awardingOrganisation);

            if (addToDbContext)
            {
                _dbContext.AddRange(ipModelTlevelCombinations);
            }
            return ipModelTlevelCombinations;
        }
    }
}
