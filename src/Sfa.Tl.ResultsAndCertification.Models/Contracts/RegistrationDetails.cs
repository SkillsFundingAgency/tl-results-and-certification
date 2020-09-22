using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class RegistrationDetails
    {
        public RegistrationDetails()
        {
            SpecialismCodes = new List<string>();
        }

        public int ProfileId { get; set; } 
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderDisplayName { get; set; }
        public string PathwayLarId { get; set; }
        public string PathwayDisplayName { get; set; }
        public IEnumerable<string> SpecialismsDisplayName{ get; set; }
        public int AcademicYear { get; set; }
        public RegistrationPathwayStatus Status { get; set; }

        // TODO: New Prop.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> SpecialismCodes { get; set; }
    }
}
