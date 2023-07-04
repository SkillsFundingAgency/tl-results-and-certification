using System.Collections.Generic;
using NSubstitute.Routing.Handlers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlDualSpecialismToSpecialismBuilder
    {
        public Domain.Models.TlDualSpecialismToSpecialism Build(EnumAwardingOrganisation awardingOrganisation)
        {
            var specialism = new TlSpecialismBuilder().BuildList(awardingOrganisation);
            var dualSpecialism = new TlDualSpecialismBuilder().BuildList(awardingOrganisation);

            return new Domain.Models.TlDualSpecialismToSpecialism
            {
                TlDualSpecialismId = dualSpecialism[0].Id,
                TlSpecialismId = specialism[0].Id,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<Domain.Models.TlDualSpecialismToSpecialism> BuildList() => new List<Domain.Models.TlDualSpecialismToSpecialism>
        {
            new Domain.Models.TlDualSpecialismToSpecialism
            {
                TlDualSpecialismId = 1,
                TlSpecialismId  = 11,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlDualSpecialismToSpecialism
            {
                TlDualSpecialismId = 1, 
                TlSpecialismId = 13,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
