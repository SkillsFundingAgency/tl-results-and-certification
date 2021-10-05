using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class AppealGradeAfterDeadlineViewModel
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        //public int ResultId { get; set; }
        public DateTime AppealEndDate { get; set; }
        public PrsStatus? PathwayPrsStatus { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public bool IsValid { get { return Status == RegistrationPathwayStatus.Active && IsAppealAllowedAfterDeadline; } }
        private bool IsAppealAllowedAfterDeadline { get { return !PathwayPrsStatus.HasValue && !CommonHelper.IsAppealsAllowed(AppealEndDate); } }

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.PrsLearnerDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, AssessmentId.ToString() } }
        };
    }
}