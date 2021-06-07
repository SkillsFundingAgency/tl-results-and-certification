namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement
{
    public class SoaPrintingRequest
    {
        public long ProviderUkprn { get; set; }
        public int AddressId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public LearningDetails LearningDetails { get; set; }
        public SoaPrintingDetails SoaPrintingDetails { get; set; }
        public string PerformedBy { get; set; }
    }
}