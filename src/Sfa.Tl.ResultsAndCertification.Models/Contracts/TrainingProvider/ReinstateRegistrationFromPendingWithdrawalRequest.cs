namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class ReinstateRegistrationFromPendingWithdrawalRequest
    {
        public int ProfileId { get; set; }

        public long AoUkprn { get; set; }

        public string PerformedBy { get; set; }
    }
}
