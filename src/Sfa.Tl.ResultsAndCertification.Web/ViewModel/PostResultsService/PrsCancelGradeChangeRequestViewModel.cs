using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
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
        public PrsStatus PathwayPrsStatus { get; set; }

        public bool IsValid { get { return Status == RegistrationPathwayStatus.Active && PathwayPrsStatus == PrsStatus.Final; } }

        [Required(ErrorMessageResourceType = typeof(ValidationContent.PrsCancelGradeChangeRequest), ErrorMessageResourceName = "Validation_Message")]
        public bool? AreYouSureToCancel { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.PrsLearnerDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, AssessmentId.ToString() } }
        };
    }
}
