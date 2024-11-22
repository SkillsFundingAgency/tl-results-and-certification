namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider
{
    public class AddProviderRequest
    {
        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string CreatedBy { get; set; }
    }
}