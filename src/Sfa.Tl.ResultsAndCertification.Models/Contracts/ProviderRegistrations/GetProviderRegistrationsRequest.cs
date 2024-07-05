namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderRegistrations
{
    public class GetProviderRegistrationsRequest
    {
        public long ProviderUkprn { get; set; }

        public int StartYear { get; set; }

        public string RequestedBy { get; set; }
    }
}