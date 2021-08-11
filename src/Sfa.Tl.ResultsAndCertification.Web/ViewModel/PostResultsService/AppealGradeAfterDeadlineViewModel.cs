using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class AppealGradeAfterDeadlineViewModel
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public DateTime AppealEndDate { get; set; }
        public PrsStatus? PathwayPrsStatus { get; set; }        
        public RegistrationPathwayStatus Status { get; set; }
        public bool IsValid { get { return Status == RegistrationPathwayStatus.Active && IsAppealAllowedAfterDeadline; } }
        private bool IsAppealAllowedAfterDeadline { get { return !PathwayPrsStatus.HasValue && !CommonHelper.IsAppealsAllowed(AppealEndDate); } }
    }
}