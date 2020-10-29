using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess
{
    public class AssessmentCsvRecordResponse : ValidationState<RegistrationValidationError>
    {
        // TODO:
        public int RowNum { get; set; }

        public long Uln { get; set; }
    }
}
