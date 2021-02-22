using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class RegistrationLearnerDetails
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }        
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public bool? IsLearnerVerified { get; set; }
        public bool? IsEnglishAndMathsAchieved { get; set; }
        public bool? IsSendLearner { get; set; }
        public bool? IsRcFeed { get; set; }
    }
}