namespace Sfa.Tl.ResultsAndCertification.Models.Certificates
{
    public class CertificateDataResponse
    {
        public bool IsSuccess { get; set; }
        public int BatchId { get; set; }
        public int TotalBatchRecordsCreated { get; set; }
        public int OverallResultsUpdatedCount { get; set; }
    }
}
