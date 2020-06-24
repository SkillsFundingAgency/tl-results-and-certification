namespace Sfa.Tl.ResultsAndCertification.Models.BulkProcess
{
    public class BulkUploadStats
    {
        public int TotalRecordsCount { get; set; }
        public int NewRecordsCount { get; set; }
        public int UpdatedRecordsCount { get; set; }
        public int DuplicateRecordsCount { get; set; }
    }
}
