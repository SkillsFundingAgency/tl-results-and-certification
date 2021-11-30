using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner
{
    public class LearnerRecord
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public Pathway Pathway { get; set; }
    }
}