namespace Sfa.Tl.ResultsAndCertification.Application.Models
{
    internal class OverallResultBatchResponse
    {
        public int TotalRecords { get; set; }
        public int UpdatedRecords { get; set; }
        public int NewRecords { get; set; }
        public int UnChangedRecords { get; set; }

        public bool? IsSuccess { get; set; }
    }
}
