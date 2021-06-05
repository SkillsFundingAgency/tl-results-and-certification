namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement
{
    public class SoaPrintingRequest
    {
        public int PostalAddressId { get; set; }
        public int TqRegistrationPathwayId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public LearningDetails LearningDetails { get; set; }
        public SoaPrintingDetails SoaPrintingDetails { get; set; }
        public string PerformedBy { get; set; }
    }
}