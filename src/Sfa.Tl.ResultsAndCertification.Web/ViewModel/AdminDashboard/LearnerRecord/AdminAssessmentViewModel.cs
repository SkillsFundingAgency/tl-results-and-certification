using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.TableButton;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord
{
    public class AdminAssessmentViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int AssessmentId { get; set; }

        public ComponentType ComponentType { get; set; }

        public string ExamPeriod { get; set; }

        public bool HasGrade
            => !string.IsNullOrWhiteSpace(Grade);

        public string Grade { get; set; }

        public string PrsDisplayText { get; set; }

        public string LastUpdated { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsResultChangeAllowed { get; set; }

        public TableButtonModel ActionButton { get; set; }
    }
}