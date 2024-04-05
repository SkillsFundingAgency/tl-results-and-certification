using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewAddSpecialismAssessmentRequest:ReviewChangeRequest
    {
        public AddSpecialismDetails AddSpecialismDetails { get; set; }

        public override ChangeType ChangeType => ChangeType.AddSpecialismAssessment;

        public int SpecialismId { get; set; }
    }
}
