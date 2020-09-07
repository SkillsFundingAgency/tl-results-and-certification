using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class RegistrationDetails
    {
        public int ProfileId { get; set; } 
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderDisplayName { get; set; }
        public string PathwayDisplayName { get; set; }
        public IEnumerable<string> SpecialismsDisplayName{ get; set; }
        public int AcademicYear { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
    }
}
