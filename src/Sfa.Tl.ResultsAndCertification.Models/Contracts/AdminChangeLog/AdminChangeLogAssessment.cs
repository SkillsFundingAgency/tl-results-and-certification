using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog
{
    public class AdminChangeLogAssessment
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
        public IEnumerable<AdminChangeLogResult> Results { get; set; }
    }
}