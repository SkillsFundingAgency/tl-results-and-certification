using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class VerifyTlevelViewModel
    {
        public VerifyTlevelViewModel()
        {
            Specialisms = new List<string>();
        }

        public int TqAwardingOrganisationId { get; set; }
        public int RouteId { get; set; }
        public int PathwayId { get; set; }
        public string PathwayName { get; set; }        
        public bool? IsEverythingCorrect { get; set; }
        public IEnumerable<string> Specialisms { get; set; }
    }
}
