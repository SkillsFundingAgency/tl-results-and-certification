using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewRemoveAssessmentEntryRequest : ReviewChangeRequest
    {
        public int AssessmentId { get; set; }

        public DetailsChangeAssessmentRemove ChangeAssessmentDetails { get; set; }

        public DetailsSpecialismAssessmentRemove ChangeSpecialismAssessmentDetails { get; set; }

        public ComponentType ComponentType { get; set; }

        public override ChangeType ChangeType => ChangeType.RemovePathwayAssessment;
    }
}
