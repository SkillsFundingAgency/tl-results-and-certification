using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model
{
    public class RegistrationValidationError
    {
        [DisplayName("Line")]
        public string RowNum { get; set; }
        
        [DisplayName("ULN")]
        public string Uln { get; set; }
        
        [DisplayName("Error")]
        public string ErrorMessage { get; set; }
    }
}