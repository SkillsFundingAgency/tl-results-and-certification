using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class BatchSummary
    {
        public int BatchNumber { get; set; }
        public DateTime BatchDate { get; set; }
        public DateTime ProcessedDate { get; set; }
        public int PostalContactCount { get; set; }
        public int TotalCertificateCount { get; set; }
        public string Status { get; set; }
        public DateTime StatusChangeDate { get; set; }
        public string ErrorMessage { get; set; }
    }
}
