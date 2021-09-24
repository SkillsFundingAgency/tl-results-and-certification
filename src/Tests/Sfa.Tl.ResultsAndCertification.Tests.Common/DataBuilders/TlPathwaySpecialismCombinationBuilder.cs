using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlPathwaySpecialismCombinationBuilder
    {
        public Domain.Models.TlPathwaySpecialismCombination Build(EnumAwardingOrganisation awardingOrganisation)
        {
            var pathway = new TlPathwayBuilder().Build(awardingOrganisation);
            var specialism = new TlSpecialismBuilder().BuildList(awardingOrganisation);
            return new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = pathway.Id,
                TlSpecialismId = specialism[0].Id,
                GroupId = 1,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }        

        public IList<Domain.Models.TlPathwaySpecialismCombination> BuildList() => new List<Domain.Models.TlPathwaySpecialismCombination>
        {
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 1,
                TlSpecialismId = 1,
                GroupId = 1,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 1,
                TlSpecialismId = 2,
                GroupId = 1,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 1,
                TlSpecialismId = 3,
                GroupId = 2,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 1,
                TlSpecialismId = 4,
                GroupId = 2,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 2,
                TlSpecialismId = 5,
                GroupId = 1,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 2,
                TlSpecialismId = 6,
                GroupId = 1,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 3,
                TlSpecialismId = 7,
                GroupId = 1,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 3,
                TlSpecialismId = 8,
                GroupId = 1,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
