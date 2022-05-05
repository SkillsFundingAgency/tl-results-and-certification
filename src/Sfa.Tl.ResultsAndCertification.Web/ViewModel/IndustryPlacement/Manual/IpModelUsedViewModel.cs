using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpModelUsedViewModel
    {
        public int ProfileId { get; set; }
        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.IpModelUsed), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsIpModelUsed { get; set; }
        public virtual BackLinkModel BackLink => new() { RouteName = RouteConstants.IpCompletion, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } };
    }
}
