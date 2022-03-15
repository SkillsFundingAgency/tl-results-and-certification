using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ValidationContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsCancelGradeChangeRequestViewModel
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime AppealEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationContent.PrsCancelGradeChangeRequest), ErrorMessageResourceName = "Validation_Message")]
        public bool? AreYouSureToCancel { get; set; }
        public bool IsResultJourney { get; set; }

        public bool IsValid
        {
            get
            {
                return Status == RegistrationPathwayStatus.Active &&
                       (PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.Final || !CommonHelper.IsAppealsAllowed(AppealEndDate));
            }
        }

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.PrsGradeChangeRequest,
            RouteAttributes = IsResultJourney
                    ? new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, AssessmentId.ToString() }, { Constants.IsResultJourney, true.ToString() } }
                    : new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, AssessmentId.ToString() } }
        };
    }
}
