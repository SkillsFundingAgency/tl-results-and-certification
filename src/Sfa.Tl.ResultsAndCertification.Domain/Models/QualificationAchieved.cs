namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class QualificationAchieved : BaseEntity
    {        
        public int TqRegistrationProfileId { get; set; }
        public int QualificationId { get; set; }
        public int QualificationGradeId { get; set; }       
        public bool IsAchieved { get; set; }

        public virtual TqRegistrationProfile TqRegistrationProfile { get; set; }
        public virtual Qualification Qualification{ get; set; }
        public virtual QualificationGrade QualificationGrade { get; set; }
    }
}