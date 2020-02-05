namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlPathwaySpecialismMar : BaseEntity
    {
        public int TlMandatoryAdditionalRequirementId { get; set; }
        public int? TlPathwayId { get; set; }
        public int? TlSpecialismId { get; set; }

        public virtual TlMandatoryAdditionalRequirement TlMandatoryAdditionalRequirement { get; set; }
        public virtual TlPathway TlPathway { get; set; }
        public virtual TlSpecialism TlSpecialism { get; set; }
    }
}
