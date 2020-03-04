using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqAwardingOrganisationBuilder
    {
        public TqAwardingOrganisation Build(EnumAwardingOrganisation awardingOrganisation, bool addDefaultNavigationData = true)
        {
            var tlAwardingOrganisation = addDefaultNavigationData ? new TlAwardingOrganisationBuilder().Build(awardingOrganisation) : null;
            var route = addDefaultNavigationData ? new TlRouteBuilder().Build(awardingOrganisation) : null;
            var pathway = addDefaultNavigationData ? new TlPathwayBuilder().Build(awardingOrganisation) : null;

            return new TqAwardingOrganisation
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

        public IList<TqAwardingOrganisation> BuildList(EnumAwardingOrganisation awardingOrganisation, bool addDefaultNavigationData = true)
        {
            var results = new List<TqAwardingOrganisation>();
            var tlAwardingOrganisation = addDefaultNavigationData ? new TlAwardingOrganisationBuilder().Build(awardingOrganisation) : null;
            var pathways = new TlPathwayBuilder().BuildList(awardingOrganisation);

            foreach (var pathway in pathways)
            {
                results.Add(new TqAwardingOrganisation
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

        public IList<TqAwardingOrganisation> BuildList(EnumAwardingOrganisation awardingOrganisation, TlAwardingOrganisation tlAwardingOrganisation = null, IList<TlPathway> tlPathways = null, TlevelReviewStatus tlevelReviewStatus = TlevelReviewStatus.AwaitingConfirmation)
        {
            var results = new List<TqAwardingOrganisation>();
            var tlAwardingOrg = tlAwardingOrganisation ?? new TlAwardingOrganisationBuilder().Build(awardingOrganisation);
            var pathways = tlPathways ?? new TlPathwayBuilder().BuildList(awardingOrganisation);

            foreach (var pathway in pathways)
            {
                results.Add(new TqAwardingOrganisation
                {
                    TlAwardingOrganisaton = tlAwardingOrg,
                    TlPathway = pathway,
                    TlRoute = pathway.TlRoute,
                    ReviewStatus = (int)tlevelReviewStatus,
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
