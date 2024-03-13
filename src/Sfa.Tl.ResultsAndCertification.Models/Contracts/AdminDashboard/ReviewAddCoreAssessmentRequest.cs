using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewAddCoreAssessmentRequest: ReviewChangeRequest
    {
        public AddCoreAssessmentDetails AddCoreAssessmentDetails { get; set; }
        public override ChangeType ChangeType => ChangeType.AssessmentEntryAdd;
    }
}
