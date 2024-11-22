using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider
{
    public class GetProviderResponse
    {
        public int ProviderId { get; set; }
        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
    }
}