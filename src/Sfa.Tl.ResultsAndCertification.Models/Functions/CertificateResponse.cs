namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class CertificateResponse : FunctionResponse
    {
        public int TotalRecords { get; set; }
        public int UpdatedRecords { get; set; }
        public int SavedRecords { get; set; }

        
        // TODO: Reconsile Once Save and Update are implemented.
        public int BatchId { get; set; }
        public int ProvidersCount { get; set; }
        public int CertificatesCreated { get; set; }
        public int OverallResultsUpdatedCount { get; set; }
    }
}
