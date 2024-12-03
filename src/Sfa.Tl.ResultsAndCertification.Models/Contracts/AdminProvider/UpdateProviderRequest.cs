namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider
{
    public class UpdateProviderRequest
    {
        public int ProviderId { get; set; }
        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public string ModifiedBy { get; set; }
    }
}