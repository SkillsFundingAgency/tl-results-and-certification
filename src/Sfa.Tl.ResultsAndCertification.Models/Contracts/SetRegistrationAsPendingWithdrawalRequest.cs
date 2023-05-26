namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class SetRegistrationAsPendingWithdrawalRequest
    {
        public int ProfileId { get; set; }
        public long AoUkprn { get; set; }
        public bool IsPendingWithdrawl { get; set; }
        public string PerformedBy { get; set; }
    }
}
