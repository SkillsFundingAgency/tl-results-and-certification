namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class OverallResultResponse : FunctionResponse
    {
        public int TotalRecords { get; set; }
        public int UpdatedRecords { get; set; }
        public int NewRecords { get; set; }
        public int UnChangedRecords { get; set; }
        public int SavedRecords { get; set; }
    }
}
