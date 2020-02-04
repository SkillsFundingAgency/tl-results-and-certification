using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlPathwaySpecialismCombinationBuilder
    {
        public Domain.Models.TlPathwaySpecialismCombination Build() => new Domain.Models.TlPathwaySpecialismCombination
        {
            TlPathway = new TlPathwayBuilder().Build(),
            TlSpecialism = new TlSpecialismBuilder().Build(),
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
                TlPathwayId = 1,
                TlSpecialismId = 1,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 1,
                TlSpecialismId = 2,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 1,
                TlSpecialismId = 3,
                Group = "G2",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 1,
                TlSpecialismId = 4,
                Group = "G2",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 2,
                TlSpecialismId = 5,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 2,
                TlSpecialismId = 6,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 2,
                TlSpecialismId = 7,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismCombination
            {
                TlPathwayId = 3,
                TlSpecialismId = 8,
                Group = "G1",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
