using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement
{
    public class PrintRequestSnapshot
    {
        public int ProfileId { get; set; }
        public string RequestDetails { get; set; }
        public RegistrationPathwayStatus RegistrationPathwayStatus { get; set; }
        public DateTime RequestedDate { get; set; }
        public string RequestedBy { get; set; }
    }
}
