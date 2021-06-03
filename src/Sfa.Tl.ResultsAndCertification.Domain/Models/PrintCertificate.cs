using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class PrintCertificate : BaseEntity
    {
        public int PrintBatchItemId { get; set; }
        public int TqRegistrationPathwayId { get; set; }
        public string CertificateNumber { get; set; }
        public string LearnerName { get; set; }
        public string LearningDetails { get; set; }
        public string DisplaySnapshot { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
        public string TrackingId { get; set; }
        public DateTime StatusChangedOn { get; set; }

        public virtual PrintBatchItem PrintBatchItem { get; set; }
        public virtual TqRegistrationPathway TqRegistrationPathway { get; set; }
    }
}
