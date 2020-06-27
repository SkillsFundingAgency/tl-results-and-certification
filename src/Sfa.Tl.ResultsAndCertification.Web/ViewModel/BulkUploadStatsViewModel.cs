namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class BulkUploadStatsViewModel
    {
        public int TotalRecordsCount { get; set; }
        public int NewRecordsCount { get; set; }
        public int AmendedRecordsCount { get; set; }
        public int UnchangedRecordsCount { get; set; }
    }
}
