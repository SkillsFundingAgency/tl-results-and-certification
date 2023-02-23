using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class SpecialConsiderationHoursViewModel
    {
        public int ProfileId { get; set; }
        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.IpSpecialConsiderationHours), ErrorMessageResourceName = "Hours_Required_Validation_Message")]
        [RegularExpression(Constants.IpSpecialConsiderationHoursRegex, ErrorMessageResourceType = typeof(ErrorResource.IpSpecialConsiderationHours), ErrorMessageResourceName = "Hours_Must_Be_Between_1_999")]
        public string Hours { get; set; }
        public bool IsChangeJourney { get; set; }
        public bool IsChangeMode { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                var journeyRoute = IsChangeJourney ? RouteConstants.IpCompletionChange : RouteConstants.IpCompletion;
                return IsChangeMode ? new() { RouteName = RouteConstants.IpCheckAndSubmit } : new() { RouteName = journeyRoute, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } };
            }
        }
    }
}
