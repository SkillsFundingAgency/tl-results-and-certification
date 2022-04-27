using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class IpAchievedBuilder
    {
        public IpAchieved Build()
        {
            var industryPlacement = new IndustryPlacementBuilder().Build();
            var ipLookup = new IpLookupBuilder().Build();

            return new IpAchieved
            {
                IndustryPlacementId = industryPlacement.Id,
                IpLookupId = ipLookup.Id,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<IpAchieved> BuildList()
        {
            var industryPlacement = new IndustryPlacementBuilder().Build();
            var ipLookup = new IpLookupBuilder().BuildList();

            var ipAchievedList = new List<IpAchieved>()
            {
                new IpAchieved
                {
                    IndustryPlacementId = industryPlacement.Id,
                    IpLookupId = ipLookup[0].Id,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new IpAchieved
                {
                    IndustryPlacementId = industryPlacement.Id,
                    IpLookupId = ipLookup[1].Id,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return ipAchievedList;
        }
    }
}
