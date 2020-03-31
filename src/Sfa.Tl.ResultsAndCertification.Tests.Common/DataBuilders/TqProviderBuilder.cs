using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqProviderBuilder
    {
        public Domain.Models.TqProvider Build() => new Domain.Models.TqProvider
        {
            TqAwardingOrganisationId = 1,
            TlProviderId = 2,
            TlPathwayId = 1,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TqProvider> BuildList() => new List<Domain.Models.TqProvider>
        {
            new Domain.Models.TqProvider
            {
                TqAwardingOrganisationId = 1,
                TlProviderId = 2,
                TlPathwayId = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqProvider
            {
                TqAwardingOrganisationId = 2,
                TlProviderId = 2,
                TlPathwayId = 2,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqProvider
            {
                TqAwardingOrganisationId = 3,
                TlProviderId = 2,
                TlPathwayId = 3,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
