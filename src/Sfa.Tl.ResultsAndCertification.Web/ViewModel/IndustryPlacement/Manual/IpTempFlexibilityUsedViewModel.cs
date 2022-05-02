using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;

using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpTempFlexibilityUsedViewModel
    {
        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.IpTempFlexibilityUsed), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsTempFlexibilityUsed { get; set; }

        // TODO: Two routes. 
        public virtual BackLinkModel BackLink => new() { RouteName = RouteConstants.IpModelUsed };
    }
}