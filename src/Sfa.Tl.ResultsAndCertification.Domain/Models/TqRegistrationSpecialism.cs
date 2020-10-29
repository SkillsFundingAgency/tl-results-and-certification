using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqRegistrationSpecialism : BaseEntity
    {
        public TqRegistrationSpecialism()
        {
            TqSpecialismAssessments = new HashSet<TqSpecialismAssessment>();
        }

        public int TqRegistrationPathwayId { get; set; }
        public int TlSpecialismId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsOptedin { get; set; }
        public bool IsBulkUpload { get; set; }

        public virtual TlSpecialism TlSpecialism { get; set; }
        public virtual TqRegistrationPathway TqRegistrationPathway { get; set; }
        public virtual ICollection<TqSpecialismAssessment> TqSpecialismAssessments { get; set; }
    }
}
