using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class RegistrationDetails
    {
        public RegistrationDetails()
        {
            Specialisms = new List<SpecialismDetails>();
        }

        public long Uln { get; set; }
        public long AoUkprn { get; set; }
        public int ProfileId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }
        public string PathwayLarId { get; set; }
        public string PathwayName { get; set; }
        public IEnumerable<SpecialismDetails> Specialisms { get; set; }
        public int AcademicYear { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
    }
}
