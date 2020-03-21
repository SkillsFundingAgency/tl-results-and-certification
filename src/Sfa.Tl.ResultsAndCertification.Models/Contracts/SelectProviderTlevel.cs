using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class SelectProviderTlevel
    {
        public int TqAwardingOrganisationId { get; set; }
        public int TlProviderId { get; set; }
        public int TlPathwayId { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
    }
}
