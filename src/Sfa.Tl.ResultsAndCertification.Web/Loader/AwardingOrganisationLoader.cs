using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AwardingOrganisationLoader : IAwardingOrganisationLoader
    {

        // TODO: Inject ApiClient 
        public AwardingOrganisationLoader()
        {
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByAwardingOrganisationAsync(int id)
        {
            var result = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { TlPathwayId = 1, TlRouteId = 1, Route = new Route { Name = "Route 1"}, Pathway = new Pathway { Name = "Pathway 1" }, ReviewStatus = 1 },
                new AwardingOrganisationPathwayStatus { TlPathwayId = 2, TlRouteId = 1, Route = new Route { Name = "Route 1"}, Pathway = new Pathway { Name = "Pathway 2" }, ReviewStatus = 2 },
                new AwardingOrganisationPathwayStatus { TlPathwayId = 1, TlRouteId = 2, Route = new Route { Name = "Route 2"}, Pathway = new Pathway { Name = "Pathway 1" }, ReviewStatus = 3 },
            };

            return await Task.Run(() => result);
        }
    }
}
