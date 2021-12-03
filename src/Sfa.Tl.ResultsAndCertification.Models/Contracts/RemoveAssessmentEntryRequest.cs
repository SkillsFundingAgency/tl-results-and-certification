using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class RemoveAssessmentEntryRequest
    {
        public int AssessmentId { get; set; }

        public long AoUkprn { get; set; }

        public IList<int?> SpecialismAssessmentIds { get; set; }

        public ComponentType ComponentType { get; set; }

        public string PerformedBy { get; set; }
    }
}