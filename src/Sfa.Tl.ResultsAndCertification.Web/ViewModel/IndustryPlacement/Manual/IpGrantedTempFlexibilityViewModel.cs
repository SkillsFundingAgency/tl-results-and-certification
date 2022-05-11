using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpGrantedTempFlexibilityViewModel
    {
        public IpGrantedTempFlexibilityViewModel()
        {
            TemporaryFlexibilities = new List<IpLookupDataViewModel>();
        }

        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.IpGrantedTempFlexibility), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsTempFlexibilitySelected => (TemporaryFlexibilities.Any(x => x.IsSelected) == true) ? true : null;

        public IList<IpLookupDataViewModel> TemporaryFlexibilities { get; set; }

        public virtual BackLinkModel BackLink { get; set; }

        public void SetBackLink(IpTempFlexibilityViewModel tempFlexibilityModel)
        {
            if (tempFlexibilityModel?.IpBlendedPlacementUsed?.IsBlendedPlacementUsed == false)
            {
                BackLink = new BackLinkModel { RouteName = RouteConstants.IpBlendedPlacementUsed };
            }
            else
            {
                BackLink = new BackLinkModel { RouteName = RouteConstants.IpTempFlexibilityUsed };
            }
        }
    }
}
