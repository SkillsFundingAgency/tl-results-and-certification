using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class IpTempFlexTlevelCombinationBuilder
    {
        public Domain.Models.IpTempFlexTlevelCombination Build(EnumAwardingOrganisation awardingOrganisation)
        {
            var pathway = new TlPathwayBuilder().Build(awardingOrganisation);
            var ipLookup = new IpLookupBuilder().Build();

            return new Domain.Models.IpTempFlexTlevelCombination
            {
                TlPathwayId = pathway.Id,
                IpLookupId = ipLookup.Id,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<Domain.Models.IpTempFlexTlevelCombination> BuildList(EnumAwardingOrganisation awardingOrganisation)
        {
            var pathway = new TlPathwayBuilder().Build(awardingOrganisation);
            var ipLookup = new IpLookupBuilder().BuildList();

            var ipTempFlexCombinations = new List<Domain.Models.IpTempFlexTlevelCombination>()
            {
                new Domain.Models.IpTempFlexTlevelCombination
                {
                    TlPathwayId = pathway.Id,
                    IpLookupId = ipLookup[0].Id,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpTempFlexTlevelCombination
                {
                    TlPathwayId = pathway.Id,
                    IpLookupId = ipLookup[1].Id,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpTempFlexTlevelCombination
                {
                    TlPathwayId = pathway.Id,
                    IpLookupId = ipLookup[2].Id,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpTempFlexTlevelCombination
                {
                    TlPathwayId = pathway.Id,
                    IpLookupId = ipLookup[3].Id,
                    IsActive = false,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
            };
            return ipTempFlexCombinations;
        }
    }
}
