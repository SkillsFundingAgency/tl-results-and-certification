namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AwardingOrganisationMetadata : BaseModel
    {
        public long Ukprn { get; set; }

        public string DisplayName { get; set; }
    }
}
