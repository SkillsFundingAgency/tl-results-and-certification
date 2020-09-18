namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class WithdrawRegistrationRequest
    {
        public int ProfileId { get; set; }

        public long AoUkprn { get; set; }

        public string PerformedBy { get; set; }
    }
}
