namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class WithdrawWithdrawlRequest
    {
        public int ProfileId { get; set; }

        public long AoUkprn { get; set; }

        public string PerformedBy { get; set; }
    }
}
