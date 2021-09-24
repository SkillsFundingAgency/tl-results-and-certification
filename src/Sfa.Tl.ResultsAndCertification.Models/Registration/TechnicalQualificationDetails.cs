using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration
{
    public class TechnicalQualificationDetails
    {
        public TechnicalQualificationDetails()
        {
            TlSpecialismLarIds = new List<KeyValuePair<int, string>>();
            TlSpecialismCombinations = new List<KeyValuePair<int, string>>();
        }
        public long ProviderUkprn { get; set; }
        public int TlPathwayId { get; set; }
        public string PathwayLarId { get; set; }
        public string PathwayName { get; set; }
        public int TqProviderId { get; set; }
        public int TlProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public int TlAwardingOrganisatonId { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismLarIds { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismCombinations { get; set; }
    }
}
