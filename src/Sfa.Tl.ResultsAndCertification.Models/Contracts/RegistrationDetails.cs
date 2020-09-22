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
        public string Name { get; set; }  // TODO: delete
        public DateTime DateofBirth { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }
        public string PathwayLarId { get; set; }
        public string PathwayName { get; set; }
        public IEnumerable<string> SpecialismsDisplayName{ get; set; } // TODO: delete
        public int AcademicYear { get; set; }
        public RegistrationPathwayStatus Status { get; set; }

        // TODO: New Prop.
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public IEnumerable<string> SpecialismCodes { get; set; } // Not in use (ManageReg to use below if required)
        public IEnumerable<SpecialismDetails> Specialisms { get; set; }
    }
}
