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
        public bool IsChangeMode { get; set; }
        public BackLinkModel BackLink { get; set; }

        public void SetBackLink(IpModelViewModel ipModel)
        {
            if (IsChangeMode)
                BackLink = new BackLinkModel { RouteName = RouteConstants.IpCheckAndSubmit };
            else if (ipModel.IpModelUsed.IsIpModelUsed == true)
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