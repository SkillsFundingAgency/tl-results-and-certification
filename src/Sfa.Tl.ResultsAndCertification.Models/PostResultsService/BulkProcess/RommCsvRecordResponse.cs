using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess
{
    public class RommCsvRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public int RowNum { get; set; }

        public long Uln { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public long ProviderUkprn { get; set; }

        public int AcademicYear { get; set; }

        public string AcademicYearName { get; set; }

        public string CoreAssessmentSeries { get; set; }

        public string CoreCode { get; set; }

        public bool CoreRommOpen { get; set; }

        public string CoreRommOutcome { get; set; }

        public string SpecialismAssessmentSeries { get; set; }

        public string SpecialismCode { get; set; }

        public bool SpecialismRommOpen { get; set; }

        public string SpecialismRommOutcome { get; set; }
    }
}
