using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class DualSpecialismOverallGradeLookup : BaseEntity
    {
        public int FirstTlLookupSpecialismGradeId { get; set; }

        public int SecondTlLookupSpecialismGradeId { get; set; }

        public int TlLookupOverallSpecialismGradeId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("FirstTlLookupSpecialismGradeId")]
        public virtual TlLookup FirstTlLookupSpecialismGrade { get; set; }

        [ForeignKey("SecondTlLookupSpecialismGradeId")]
        public virtual TlLookup SecondTlLookupSpecialismGrade { get; set; }

        [ForeignKey("TlLookupOverallSpecialismGradeId")]
        public virtual TlLookup TlLookupOverallSpecialismGrade { get; set; }
    }
}