using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class IpAchievedProvider
    {
        public static IpAchieved CreateIpAchieved(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var ipAchieved = new IpAchievedBuilder().Build();

            if (addToDbContext && ipAchieved != null)
            {
                _dbContext.Add(ipAchieved);
            }
            return ipAchieved;
        }

        public static IpAchieved CreateIpAchieved(ResultsAndCertificationDbContext _dbContext, IpAchieved ipAchieved, bool addToDbContext = true)
        {
            if (addToDbContext && ipAchieved == null)
            {
                _dbContext.Add(ipAchieved);
            }
            return ipAchieved;
        }

        public static IpAchieved CreateIpAchieved(ResultsAndCertificationDbContext _dbContext, IndustryPlacement industryPlacement, IpLookup ipLookup, bool addToDbContext = true)
        {
            if (industryPlacement != null)
            {
                var ipAchieved = new IpAchieved
                {
                    IndustryPlacementId = industryPlacement.Id,
                    IpLookupId = ipLookup.Id,
                    IndustryPlacement = industryPlacement,
                    IpLookup = ipLookup,
                    IsActive = true
                };

                if (addToDbContext)
                {
                    _dbContext.Add(ipAchieved);
                }
                return ipAchieved;
            }
            return null;
        }

        public static IList<IpAchieved> CreateIpAchievedsList(ResultsAndCertificationDbContext _dbContext, IList<IpAchieved> ipAchieveds = null, bool addToDbContext = true)
        {
            if (ipAchieveds == null)
                ipAchieveds = new IpAchievedBuilder().BuildList();

            if (addToDbContext)
            {
                _dbContext.AddRange(ipAchieveds);
            }
            return ipAchieveds;
        }
    }
}
