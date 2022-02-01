using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqSpecialismResult : BaseEntity
    {
        public int TqSpecialismAssessmentId { get; set; }
        public int TlLookupId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public bool IsOptedin { get; set; }
        public bool IsBulkUpload { get; set; }

        public virtual TqSpecialismAssessment TqSpecialismAssessment { get; set; }
        public virtual TlLookup TlLookup { get; set; }
    }
}
