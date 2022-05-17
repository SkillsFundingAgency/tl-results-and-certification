using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpEmployerLedUsedViewModel
    {
        public IpEmployerLedUsedViewModel()
        {
            TemporaryFlexibilities = new List<IpLookupDataViewModel>();
        }

        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.IpEmployerLedUsed), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsEmployerLedUsed { get; set; }

        public IList<IpLookupDataViewModel> TemporaryFlexibilities { get; set; }

        public bool IsChangeMode { get; set; }

        public virtual BackLinkModel BackLink => new() { RouteName = IsChangeMode ? RouteConstants.IpCheckAndSubmit : RouteConstants.IpBlendedPlacementUsed };
    }
}