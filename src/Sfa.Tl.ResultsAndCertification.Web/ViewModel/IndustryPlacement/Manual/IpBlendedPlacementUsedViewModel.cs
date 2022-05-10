using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;

using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpBlendedPlacementUsedViewModel
    {
        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.IpBlendedPlacementUsed), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsBlendedPlacementUsed { get; set; }

        public virtual BackLinkModel BackLink { get; set; }

        public void SetBackLink(IpModelViewModel ipModel, bool isTempFlexApplicable)
        {
            if (isTempFlexApplicable)
                BackLink = new BackLinkModel { RouteName = RouteConstants.IpTempFlexibilityUsed };
            else
            {
                if (ipModel.IpModelUsed.IsIpModelUsed == true)
                {
                    if (ipModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed == true)
                        BackLink = new BackLinkModel { RouteName = RouteConstants.IpMultiEmployerOther };
                    else
                        BackLink = new BackLinkModel { RouteName = RouteConstants.IpMultiEmployerSelect };
                }
                else
                    BackLink = new BackLinkModel { RouteName = RouteConstants.IpModelUsed };
            }
        }
    }
}