using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration
{
    public class RegistrationProcessResponse : ValidationState<RegistrationValidationError>
    {
        public bool IsSuccess { get; set; }

        public BulkUploadStats BulkUploadStats { get; set; }
    }
}
