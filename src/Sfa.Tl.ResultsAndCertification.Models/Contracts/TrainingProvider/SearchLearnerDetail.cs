using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class SearchLearnerDetail
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public string TlevelTitle { get; set; }
        public int AcademicYear { get; set; }

        public SubjectStatus? EnglishStatus { get; set; }
        public SubjectStatus? MathsStatus { get; set; }
        public IndustryPlacementStatus? IndustryPlacementStatus { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}