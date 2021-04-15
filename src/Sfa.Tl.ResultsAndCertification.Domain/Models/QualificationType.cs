using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class QualificationType : BaseEntity
    {
        public QualificationType()
        {
            QualificationGrades = new HashSet<QualificationGrade>();
            Qualifications = new HashSet<Qualification>();
        }

        public string Name { get; set; }
        public string SubTitle { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<QualificationGrade> QualificationGrades { get; set; }
        public virtual ICollection<Qualification> Qualifications { get; set; }
    }
}
