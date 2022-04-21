using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class AddEnglishStatusViewModel
    {
        public int ProfileId { get; set; }

        public bool? IsAchieved { get; set; }

        public string LearnerName { get; set; }

        public SubjectStatus SubjectStatus { get; set; }

        public bool IsValid => SubjectStatus == SubjectStatus.NotSpecified;
        public virtual BackLinkModel BackLink => new() { RouteName = RouteConstants.LearnerRecordDetails, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } };
    }
}
