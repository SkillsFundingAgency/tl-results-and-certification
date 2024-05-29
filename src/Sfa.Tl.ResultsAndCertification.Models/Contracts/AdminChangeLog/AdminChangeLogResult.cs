using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog
{
    public class AdminChangeLogResult
    {
        public int Id { get; set; }
        public string Grade { get; set; }
        public string GradeCode { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
