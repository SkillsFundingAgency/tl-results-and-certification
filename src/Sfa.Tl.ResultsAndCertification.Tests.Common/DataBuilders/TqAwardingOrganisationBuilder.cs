using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

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

        public Domain.Models.TqAwardingOrganisation Build(EnumAwardingOrganisation awardingOrganisation, bool addDefaultNavigationData = true)
        {
            var tlAwardingOrganisation = addDefaultNavigationData ? new TlAwardingOrganisationBuilder().Build(awardingOrganisation) : null;
            var route = addDefaultNavigationData ? new TlRouteBuilder().Build(awardingOrganisation) : null;
            var pathway = addDefaultNavigationData ? new TlPathwayBuilder().Build(awardingOrganisation) : null;

            return new Domain.Models.TqAwardingOrganisation
            {
                TlAwardingOrganisaton = tlAwardingOrganisation,
                TlPathway = pathway,
                TlRoute = route,
                ReviewStatus = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<Domain.Models.TqAwardingOrganisation> BuildList(EnumAwardingOrganisation awardingOrganisation, bool addDefaultNavigationData = true)
        {
            var results = new List<Domain.Models.TqAwardingOrganisation>();
            var tlAwardingOrganisation = addDefaultNavigationData ? new TlAwardingOrganisationBuilder().Build(awardingOrganisation) : null;
            var pathways = new TlPathwayBuilder().BuildList(awardingOrganisation);

            foreach (var pathway in pathways)
            {
                results.Add(new Domain.Models.TqAwardingOrganisation
                {
                    TlAwardingOrganisaton = tlAwardingOrganisation,
                    TlPathway = pathway,
                    TlRoute = pathway.TlRoute,
                    ReviewStatus = 1,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                });
            }
            return results;
        }
    }
}
