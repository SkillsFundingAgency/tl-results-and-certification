namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public class IpTempFlexNavigation : BaseEntity
    {
        public int TlPathwayId { get; set; }
        public int AcademicYear { get; set; }
        public bool AskTempFlexibility { get; set; }
        public bool AskBlendedPlacement { get; set; }
        public bool IsActive { get; set; }

        public virtual TlPathway TlPathway { get; set; }        
    }
}
