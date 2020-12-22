using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqSpecialismAssessment : BaseEntity
    {
        public int TqRegistrationSpecialismId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsOptedin { get; set; }
        public bool IsBulkUpload { get; set; }

        public virtual TqRegistrationSpecialism TqRegistrationSpecialism { get; set; }
        public virtual AssessmentSeries AssessmentSeries { get; set; }
    }
}
