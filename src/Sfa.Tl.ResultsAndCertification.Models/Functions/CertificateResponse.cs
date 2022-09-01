namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class CertificateResponse : FunctionResponse
    {
        public int TotalRecords { get; set; }
        public int UpdatedRecords { get; set; }        
        public int SavedRecords { get; set; }
    }
}
