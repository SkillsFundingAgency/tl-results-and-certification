namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class QualificationGrade : BaseEntity
    {       
        public int QualificationTypeId { get; set; }
        public string Grade { get; set; }
        public bool IsAllowable { get; set; }
        public bool IsActive { get; set; }
        public bool IsSendGrade { get; set; }
        public virtual QualificationType QualificationType { get; set; }        
    }
}
