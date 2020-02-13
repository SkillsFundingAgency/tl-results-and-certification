using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class TlevelPathwayDetails : BaseModel
    {
        public int RouteId { get; set; }
        public int PathwayId { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
        public List<string> Specialisms { get; set; }
     
    }
}
