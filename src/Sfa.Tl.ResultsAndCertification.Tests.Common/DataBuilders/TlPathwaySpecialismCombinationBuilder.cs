using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlPathwaySpecialismCombinationBuilder
    {
        public Domain.Models.TlPathwaySpecialismCombination Build() => new Domain.Models.TlPathwaySpecialismCombination
        {
            Pathway = new TlPathwayBuilder().Build(),
            Specialism = new TlSpecialismBuilder().Build(),
            Group = "G1",
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };
        public IList<Domain.Models.TlPathwaySpecialismCombination> BuildList() => new List<Domain.Models.TlPathwaySpecialismCombination>
        {
            new Domain.Models.TlPathwaySpecialismCombination
            {
                PathwayId = 1,
                SpecialismId = 1,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                PathwayId = 1,
                SpecialismId = 2,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                PathwayId = 1,
                SpecialismId = 3,
                Group = "G2",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                PathwayId = 1,
                SpecialismId = 4,
                Group = "G2",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                PathwayId = 2,
                SpecialismId = 5,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                PathwayId = 2,
                SpecialismId = 6,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                PathwayId = 2,
                SpecialismId = 7,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                PathwayId = 3,
                SpecialismId = 8,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
