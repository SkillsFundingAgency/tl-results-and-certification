using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqSpecialismAssessment : BaseEntity
    {
        public TqSpecialismAssessment()
        {
            TqSpecialismResults = new HashSet<TqSpecialismResult>();
        }
        public int TqRegistrationSpecialismId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsOptedin { get; set; }
        public bool IsBulkUpload { get; set; }

        public virtual TqRegistrationSpecialism TqRegistrationSpecialism { get; set; }
        public virtual AssessmentSeries AssessmentSeries { get; set; }
        public virtual ICollection<TqSpecialismResult> TqSpecialismResults { get; set; }
    }
}