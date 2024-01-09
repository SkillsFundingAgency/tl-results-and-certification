using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement
{
    public class AdminIndustryPlacementSpecialConsiderationHoursViewModel
    {
        public int TqRegistrationPathwayId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AdminIndustryPlacementSpecialConsiderationHours), ErrorMessageResourceName = "Hours_Validation_Message")]
        [RegularExpression(Constants.IpSpecialConsiderationHoursRegex, ErrorMessageResourceType = typeof(ErrorResource.AdminIndustryPlacementSpecialConsiderationHours), ErrorMessageResourceName = "Hours_Validation_Message")]
        public string Hours { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel();
                //var journeyRoute = IsChangeJourney ? RouteConstants.IpCompletionChange : RouteConstants.IpCompletion;
                //return IsChangeMode ? new() { RouteName = RouteConstants.IpCheckAndSubmit } : new() { RouteName = journeyRoute, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } };
            }
        }
    }
}
