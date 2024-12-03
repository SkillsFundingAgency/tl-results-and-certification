namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider
{
    public class AddProviderResponse
    {
        public int ProviderId { get; set; }

        public bool Success { get; set; }

        public bool IsRequestValid => !DuplicatedUkprnFound && !DuplicatedNameFound && !DuplicatedDisplayNameFound;

        public bool DuplicatedUkprnFound { get; set; }

        public bool DuplicatedNameFound { get; set; }

        public bool DuplicatedDisplayNameFound { get; set; }
    }
}