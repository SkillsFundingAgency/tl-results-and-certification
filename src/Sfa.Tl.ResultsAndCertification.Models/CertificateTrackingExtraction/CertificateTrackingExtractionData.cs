using System;
using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.CertificateTrackingExtraction
{
    public class CertificateTrackingExtractionData
    {
        [DisplayName(CertificateTrackingExtractHeader.UniqueLearnerNumber)]
        public long Uln { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.TrackingId)]
        public string TrackingId { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.PrintingCertificateType)]
        public string PrintCertificateType { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.LearnerDetails)]
        public string LearnerDetails { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.BatchId)]
        public int BatchId { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.PrintBatchItemStatus)]
        public string PrintingBatchItemStatus { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.SignedForBy)]
        public string SignedForBy { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.StatusChangedOn)]
        public DateTime? StatusChangedOn { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.BatchType)]
        public string BatchType { get; set; }

        [DisplayName(CertificateTrackingExtractHeader.BatchStatus)]
        public string BatchStatus { get; set; }
    }
}
