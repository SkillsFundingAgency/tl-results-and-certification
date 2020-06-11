namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model
{
    public class ValidationError
    {
        public int RowNum { get; set; }
        public string RowRef { get; set; }
        public string ErrorMessage { get; set; }
    }
}