namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class Qualification : BaseEntity
    {
        public int QualificationTypeId { get; set; }
        public int TlLookupId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool IsSendQualification { get; set; }
        public bool IsActive { get; set; }

        public virtual QualificationType QualificationType { get; set; }
        public virtual TlLookup TlLookup { get; set; }
    }
}