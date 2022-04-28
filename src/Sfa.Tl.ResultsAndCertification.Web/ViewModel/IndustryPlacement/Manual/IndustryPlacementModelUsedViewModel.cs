using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IndustryPlacementModelUsedViewModel
    {
        public int ProfileId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.IndustryPlacementModelUsedQuestion), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsAchieved { get; set; }

        public string LearnerName { get; set; }
        public IndustryPlacementModelUsedStatus IndustryPlacementModelUsedStatus { get; set; }

        public bool IsValid => IndustryPlacementModelUsedStatus == IndustryPlacementModelUsedStatus.NotSpecified;
        public virtual BackLinkModel BackLink => new() { RouteName = RouteConstants.IpCompletion, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } };
    }
}
