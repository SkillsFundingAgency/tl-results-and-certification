using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlPathwaySpecialismMarBuilder
    {
        public Domain.Models.TlPathwaySpecialismMar Build() => new Domain.Models.TlPathwaySpecialismMar
        {
            Id = 1,
            MarId = 1,
            PathwayId = 1,
            SpecialismId = null,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TlPathwaySpecialismMar> BuildList() => new List<Domain.Models.TlPathwaySpecialismMar>
        {
            new Domain.Models.TlPathwaySpecialismMar
            {
                Id = 1,
                MarId = 1,
                PathwayId = 1,
                SpecialismId = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                Id = 2,
                MarId = 1,
                PathwayId = 2,
                SpecialismId = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                Id = 3,
                MarId = 2,
                PathwayId = null,
                SpecialismId = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                Id = 4,
                MarId = 2,
                PathwayId = null,
                SpecialismId = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                Id = 5,
                MarId = 3,
                PathwayId = null,
                SpecialismId = 3,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                Id = 6,
                MarId = 3,
                PathwayId = null,
                SpecialismId = 4,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                Id = 7,
                MarId = 4,
                PathwayId = 3,
                SpecialismId = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                Id = 8,
                MarId = 5,
                PathwayId = 3,
                SpecialismId = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
        };
    }
}
