using System;
using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner
{
    public class Assessment
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public DateTime ResultEndDate { get; set; }
        public DateTime RommEndDate { get; set; }
        public DateTime AppealEndDate { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public ComponentType ComponentType { get; set; }
        public Result Result { get; set; }
    }
}