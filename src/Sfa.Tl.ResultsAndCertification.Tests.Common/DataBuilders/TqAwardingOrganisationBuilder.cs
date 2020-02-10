using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqAwardingOrganisationBuilder
    {
        public Domain.Models.TqAwardingOrganisation Build() => new Domain.Models.TqAwardingOrganisation
        {
            TlAwardingOrganisaton = new TlAwardingOrganisationBuilder().Build(),
            TlPathway = new TlPathwayBuilder().Build(),
            TlRoute = new TlRouteBuilder().Build(),
            ReviewStatus = 1,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TqAwardingOrganisation> BuildList() => new List<Domain.Models.TqAwardingOrganisation>
        {
            new Domain.Models.TqAwardingOrganisation
            {
                TlAwardingOrganisaton = new TlAwardingOrganisationBuilder().Build(),
                TlPathway = new TlPathwayBuilder().Build(),
                TlRoute = new TlRouteBuilder().Build(),
                ReviewStatus = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
