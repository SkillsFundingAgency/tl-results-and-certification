using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqPathwayResult : BaseEntity
    {
        public int TqRegistrationAssessmentId { get; set; }
        public int TlLookupId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsOptedin { get; set; }
        public bool IsBulkUpload { get; set; }

        public virtual TqPathwayAssessment TqPathwayAssessment { get; set; }
        public virtual TlLookup TlLookup { get; set; }
    }
}