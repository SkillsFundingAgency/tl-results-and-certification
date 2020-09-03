using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class RegistrationProfile
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
    }
}
