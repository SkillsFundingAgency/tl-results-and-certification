﻿namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ProviderTlevel
    {
        public int? TqProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public int TlProviderId { get; set; }
        public string TlevelTitle { get; set; }
        public string CreatedBy { get; set; }
    }
}
