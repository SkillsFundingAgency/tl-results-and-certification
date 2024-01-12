using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement
{
    public class AdminIpSpecialConsiderationHoursViewModel
    {
        public int RegistrationPathwayId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AdminIndustryPlacementSpecialConsiderationHours), ErrorMessageResourceName = "Hours_Validation_Message")]
        [RegularExpression(Constants.IpSpecialConsiderationHoursRegex, ErrorMessageResourceType = typeof(ErrorResource.AdminIndustryPlacementSpecialConsiderationHours), ErrorMessageResourceName = "Hours_Validation_Message")]
        public string Hours { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.AdminChangeIndustryPlacement,
                    RouteAttributes = new Dictionary<string, string> { { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() } }
                };
            }
        }
    }
}