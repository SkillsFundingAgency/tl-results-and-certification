namespace Sfa.Tl.ResultsAndCertification.Models.BulkProcess
{
    public class CsvResponseBaseModel
    {
        public bool IsDirty { get; set; }
        public string ErrorMessage { get; set; }
        public CsvFileErrorCode ErrorCode { get; set; }
    }

    public enum CsvFileErrorCode
    {
        HeaderInvalid,
        NoRecordsFound
    }
}
