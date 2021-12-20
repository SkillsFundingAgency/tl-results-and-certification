using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner
{
    public class Assessment
    {
        public Assessment()
        {
            Results = new List<Result>();
        }
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public DateTime AppealEndDate { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public IEnumerable<Result> Results { get; set; }
    }
}