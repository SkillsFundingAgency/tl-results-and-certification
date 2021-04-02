namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class UpdateLearnerRecordRequest : AddLearnerRecordRequest
    {        
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public int IndustryPlacementId { get; set; }
        public bool HasIndustryPlacementChanged { get; set; }
    }
}