using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement
{
    public class PrintRequestSnapshot
    {
        public string RequestDetails { get; set; }
        public RegistrationPathwayStatus RegistrationPathwayStatus { get; set; }
        public DateTime RequestedOn { get; set; }
        public string RequestedBy { get; set; }
    }
}
