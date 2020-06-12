namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class BulkRegistrationResponse
    {
        public bool IsSuccess { get; set; }
        public string BlobErrorFileName { get; set; }
        public long ErrorFileSize { get; set; }
        public Stats Stats { get; set; }
    }

    public class Stats
    {
        // Todo: 
    }
}
