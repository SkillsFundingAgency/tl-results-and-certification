namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class SetRegistrationAsPendingWithdrawalRequest
    {
        public int ProfileId { get; set; }
        public long ProviderUkprn { get; set; }
        public string PerformedBy { get; set; }
    }
}
