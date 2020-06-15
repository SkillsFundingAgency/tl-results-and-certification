using CsvHelper.Configuration.Attributes;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model
{
    public class RegistrationValidationError
    {
        [Name("Line")] 
        public string RowNum { get; set; }
        [Name("ULN")] 
        public string Uln { get; set; }
        [Name("Error")] 
        public string ErrorMessage { get; set; }
    }
}