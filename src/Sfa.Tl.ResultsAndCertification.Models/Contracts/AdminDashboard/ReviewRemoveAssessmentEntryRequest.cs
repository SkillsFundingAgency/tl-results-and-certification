using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewRemoveCoreAssessmentEntryRequest : ReviewChangeRequest
    {
        public int AssessmentId { get; set; }

        public DetailsChangeAssessmentRemove ChangeAssessmentDetails { get; set; }

        public ComponentType ComponentType { get; set; }

        public override ChangeType ChangeType => ChangeType.RemovePathwayAssessment;
    }
}
