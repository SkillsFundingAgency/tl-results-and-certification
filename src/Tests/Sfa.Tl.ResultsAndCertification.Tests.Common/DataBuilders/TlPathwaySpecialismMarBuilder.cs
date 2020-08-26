using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlPathwaySpecialismMarBuilder
    {
        public Domain.Models.TlPathwaySpecialismMar Build() => new Domain.Models.TlPathwaySpecialismMar
        {
            TlMandatoryAdditionalRequirementId = 1,
            TlPathwayId = 1,
            TlSpecialismId = null,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TlPathwaySpecialismMar> BuildList() => new List<Domain.Models.TlPathwaySpecialismMar>
        {
            new Domain.Models.TlPathwaySpecialismMar
            {
                TlMandatoryAdditionalRequirementId = 1,
                TlPathwayId = 1,
                TlSpecialismId = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                TlMandatoryAdditionalRequirementId = 1,
                TlPathwayId = 2,
                TlSpecialismId = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                TlMandatoryAdditionalRequirementId = 2,
                TlPathwayId = null,
                TlSpecialismId = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                TlMandatoryAdditionalRequirementId = 2,
                TlPathwayId = null,
                TlSpecialismId = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                TlMandatoryAdditionalRequirementId = 3,
                TlPathwayId = null,
                TlSpecialismId = 3,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                TlMandatoryAdditionalRequirementId = 3,
                TlPathwayId = null,
                TlSpecialismId = 4,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                TlMandatoryAdditionalRequirementId = 4,
                TlPathwayId = 3,
                TlSpecialismId = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathwaySpecialismMar
            {
                TlMandatoryAdditionalRequirementId = 5,
                TlPathwayId = 3,
                TlSpecialismId = null,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
        };
    }
}
