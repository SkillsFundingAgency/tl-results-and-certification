namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class UpdateLearnerWithdrawanStatus
    {
        public int ProfileId { get; set; }
        public bool IsPendingWithdrawal { get; set; }
    }
}
