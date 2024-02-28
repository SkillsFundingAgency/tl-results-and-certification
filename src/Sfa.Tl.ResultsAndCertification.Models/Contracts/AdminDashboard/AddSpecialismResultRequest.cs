using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AddSpecialismResultRequest : ReviewChangeRequest
    {
        public int SpecialismAssessmentId { get; set; }

        public int SelectedGradeId { get; set; }

        public override ChangeType ChangeType => ChangeType.AddSpecialismResult;
    }
}