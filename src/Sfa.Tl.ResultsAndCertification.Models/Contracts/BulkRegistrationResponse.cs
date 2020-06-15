namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class BulkRegistrationResponse
    {
        public bool IsSuccess { get; set; }
        public string BlobErrorFileName { get; set; }
        public double ErrorFileSize { get; set; }
        public Stats Stats { get; set; }
    }

    public class Stats
    {
        public int NewRecordsCount { get; set; }
        public int UpdatedRecordsCount { get; set; }
        public int DuplicateRecordsCount { get; set; }
    }
}
