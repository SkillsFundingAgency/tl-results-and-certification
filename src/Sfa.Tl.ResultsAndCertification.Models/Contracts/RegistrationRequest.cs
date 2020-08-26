using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class RegistrationRequest
    {
        public RegistrationRequest()
        {
            SpecialismCodes = new List<string>();
        }

        public long AoUkprn { get; set; }

        public long Uln { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public long ProviderUkprn { get; set; }

        public string CoreCode { get; set; }

        public IEnumerable<string> SpecialismCodes { get; set; }

        public int AcademicYear { get; set; }

        public string CreatedBy { get; set; }
    }
}
