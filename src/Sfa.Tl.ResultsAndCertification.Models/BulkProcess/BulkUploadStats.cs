namespace Sfa.Tl.ResultsAndCertification.Models.BulkProcess
{
    public class BulkUploadStats
    {
        public int TotalRecordsCount { get; set; }
        public int NewRecordsCount { get; set; }
        public int AmendedRecordsCount { get; set; }
        public int UnchangedRecordsCount { get; set; }
    }
}
