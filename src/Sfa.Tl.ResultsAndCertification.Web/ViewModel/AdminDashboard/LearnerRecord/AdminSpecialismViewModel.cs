using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord
{
    public class AdminSpecialismViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int Id { get; set; }

        public string CombinedSpecialismId { get; set; }

        public string DisplayName { get; set; }

        public IEnumerable<AdminAssessmentViewModel> Assessments { get; set; }

        public bool HasAssessmentEntries
            => !Assessments.IsNullOrEmpty();
    }
}