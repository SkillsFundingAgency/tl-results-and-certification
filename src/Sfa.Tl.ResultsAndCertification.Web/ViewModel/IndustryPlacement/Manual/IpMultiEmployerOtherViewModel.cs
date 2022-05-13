using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpMultiEmployerOtherViewModel
    {
        public IpMultiEmployerOtherViewModel()
        {
            OtherIpPlacementModels = new List<IpLookupDataViewModel>();
        }

        public string LearnerName { get; set; }

        public IList<IpLookupDataViewModel> OtherIpPlacementModels { get; set; }
        public bool IsChangeMode { get; set; }

        public virtual BackLinkModel BackLink => IsChangeMode ? new() { RouteName = RouteConstants.IpCheckAndSubmit } : new() { RouteName = RouteConstants.IpMultiEmployerUsed };
    }
}
