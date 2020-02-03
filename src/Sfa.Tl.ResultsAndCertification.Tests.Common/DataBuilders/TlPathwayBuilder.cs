using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlPathwayBuilder
    {
        public Domain.Models.TlPathway Build() => new Domain.Models.TlPathway
        {
            Name = "Design, Surveying and Planning",
            LarId = "10123456",
            Route = new TlRouteBuilder().Build(),
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TlPathway> BuildList() => new List<Domain.Models.TlPathway>
        {
            new Domain.Models.TlPathway
            {
                Name = "Design, Surveying and Planning",
                LarId = "10123456",
                Route = new TlRouteBuilder().Build(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathway
            {
                Name = "Education",
                LarId = "10123456",
                Route = new TlRouteBuilder().Build(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathway
            {
                Name = "Digital Production, Design and Development",
                LarId = "10123456",
                Route = new TlRouteBuilder().Build(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
