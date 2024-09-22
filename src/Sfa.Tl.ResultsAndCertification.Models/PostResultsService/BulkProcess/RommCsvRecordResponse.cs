using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess
{
    public class RommCsvRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public RommCsvRecordResponse()
        {
            SpecialismCodes = new List<string>();
        }

        public int RowNum { get; set; }

        public long Uln { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public long ProviderUkprn { get; set; }

        public int AcademicYear { get; set; }

        public string AcademicYearName { get; set; }

        public string CoreCode { get; set; }

        public IEnumerable<string> SpecialismCodes { get; set; }
    }
}
