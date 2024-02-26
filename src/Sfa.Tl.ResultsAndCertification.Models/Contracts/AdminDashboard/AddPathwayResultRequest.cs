using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AddPathwayResultRequest : ReviewChangeRequest
    {
        public int PathwayAssessmentId { get; set; }

        public int SelectedGradeId { get; set; }

        public override ChangeType ChangeType => ChangeType.AddPathwayResult;
    }
}