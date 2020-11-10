using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqPathwayAssessment : BaseEntity
    {
        public int TqRegistrationPathwayId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsOptedin { get; set; }
        public bool IsBulkUpload { get; set; }
        
        public virtual TqRegistrationPathway TqRegistrationPathway { get; set; }
        public virtual AssessmentSeries AssessmentSeries { get; set; }
    }
}
