using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class LearnerRecordDetails
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }
        public bool IsLearnerVerified { get; set; }        
        public string PerformedBy { get; set; }

        public List<LearningEventDetails> LearningEventDetails { get; set; }
    }
}