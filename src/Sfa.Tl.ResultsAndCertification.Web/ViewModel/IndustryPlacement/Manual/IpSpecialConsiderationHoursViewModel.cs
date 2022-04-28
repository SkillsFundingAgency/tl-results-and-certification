using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpSpecialConsiderationHoursViewModel
    {
        [Range(1, 999)]
        public int Hours { get; set; }

        public virtual BackLinkModel BackLink => new()
        {
            RouteName = "TODO"
        };
    }
}
