using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpMultiEmployerSelectViewModel
    {
        public IpMultiEmployerSelectViewModel()
        {
            PlacementModels = new List<IpLookupDataViewModel>();
        }

        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.IpMultiEmployerSelect), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsIpModelSelected => (PlacementModels.Any(x => x.IsSelected) == true) ? true : null;

        public IList<IpLookupDataViewModel> PlacementModels { get; set; }

        public virtual BackLinkModel BackLink => new() { RouteName = RouteConstants.IpMultiEmployerUsed };
    }
}