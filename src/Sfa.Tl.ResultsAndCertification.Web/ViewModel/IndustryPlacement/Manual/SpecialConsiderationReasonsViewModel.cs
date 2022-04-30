using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class SpecialConsiderationReasonsViewModel
    {
        public bool? IsReasonSelected { get; set; } // TODO:
        
        public IList<IpLookupData> ReasonsList { get; internal set; }
        public virtual BackLinkModel BackLink => new()
        {
            RouteName = "TODO"
        };
    }
}
