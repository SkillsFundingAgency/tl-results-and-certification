using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess
{
    public class RegistrationValidationError
    {
        // TODO: can this be reused for both Reg and Assessment
        [DisplayName("Line")]
        public string RowNum { get; set; }

        [DisplayName("ULN")]
        public string Uln { get; set; }

        [DisplayName("Error")]
        public string ErrorMessage { get; set; }
    }
}