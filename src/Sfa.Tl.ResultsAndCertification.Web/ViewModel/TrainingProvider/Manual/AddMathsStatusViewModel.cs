using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class AddMathsStatusViewModel
    {
        public int ProfileId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddMathsStatus), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsAchieved { get; set; }

        public string LearnerName { get; set; }

        public SubjectStatus SubjectStatus { get; set; }

        public bool IsValid => SubjectStatus == SubjectStatus.NotSpecified;
        public virtual BackLinkModel BackLink => new() { RouteName = RouteConstants.LearnerRecordDetails, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } };
    }
}
