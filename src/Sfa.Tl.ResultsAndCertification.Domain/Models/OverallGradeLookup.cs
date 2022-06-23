using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class OverallGradeLookup : BaseEntity
    {
        public int TlPathwayId { get; set; }
        public int TlLookupCoreGradeId { get; set; }
        public int TlLookupSpecialismGradeId { get; set; }
        public int TlLookupOverallGradeId { get; set; }

        public virtual TlPathway TlPathway { get; set; }

        [ForeignKey("TlLookupCoreGradeId")]
        public virtual TlLookup TlLookupCoreGrade { get; set; }

        [ForeignKey("TlLookupSpecialismGradeId")]
        public virtual TlLookup TlLookupSpecialismGrade { get; set; }

        [ForeignKey("TlLookupOverallGradeId")]
        public virtual TlLookup TlLookupOverallGrade { get; set; }
    }
}
