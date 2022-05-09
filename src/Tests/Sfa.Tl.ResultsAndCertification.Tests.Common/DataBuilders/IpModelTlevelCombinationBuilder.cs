using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class IpModelTlevelCombinationBuilder
    {
        public Domain.Models.IpModelTlevelCombination Build(EnumAwardingOrganisation awardingOrganisation)
        {
            var pathway = new TlPathwayBuilder().Build(awardingOrganisation);
            var ipLookup = new IpLookupBuilder().Build();

            return new Domain.Models.IpModelTlevelCombination
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

        public IList<Domain.Models.IpModelTlevelCombination> BuildList(EnumAwardingOrganisation awardingOrganisation, IpLookupType? lookupType = null, Domain.Models.TlPathway pathway = null)
        {
            pathway = pathway ?? new TlPathwayBuilder().Build(awardingOrganisation);
            var ipLookup = new IpLookupBuilder().BuildList(lookupType);

            var ipModelCombinations = new List<Domain.Models.IpModelTlevelCombination>()
            {
                new Domain.Models.IpModelTlevelCombination
                {
                    TlPathwayId = pathway.Id,
                    TlPathway = pathway,
                    IpLookupId = ipLookup[0].Id,
                    IpLookup = ipLookup[0],
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpModelTlevelCombination
                {
                    TlPathwayId = pathway.Id,
                    TlPathway = pathway,
                    IpLookupId = ipLookup[1].Id,
                    IpLookup = ipLookup[1],
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpModelTlevelCombination
                {
                    TlPathwayId = pathway.Id,
                    TlPathway = pathway,
                    IpLookupId = ipLookup[2].Id,
                    IpLookup = ipLookup[2],
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpModelTlevelCombination
                {
                    TlPathwayId = pathway.Id,
                    TlPathway = pathway,
                    IpLookupId = ipLookup[3].Id,
                    IpLookup = ipLookup[3],
                    IsActive = false,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
            };
            return ipModelCombinations;
        }
    }
}
