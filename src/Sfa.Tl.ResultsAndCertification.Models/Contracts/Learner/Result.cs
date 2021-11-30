using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner
{
    public class Result
    {
        public int Id { get; set; }
        public string Grade { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
