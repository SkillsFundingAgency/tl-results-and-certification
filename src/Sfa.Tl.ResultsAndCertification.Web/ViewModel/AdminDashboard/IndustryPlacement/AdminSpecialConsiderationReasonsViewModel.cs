using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement
{
    public class AdminIndustryPlacementSpecialConsiderationReasonsViewModel
    {
        public int TqRegistrationPathwayId { get; set; }

        public IList<IpLookupDataViewModel> ReasonsList { get; set; } = new List<IpLookupDataViewModel>();

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AdminIndustryPlacementSpecialConsiderationReasons), ErrorMessageResourceName = "Validation_Message_Select_One_Or_More_Reasons")]
        public bool IsReasonSelected
            => !ReasonsList.IsNullOrEmpty() && ReasonsList.Any(p => p.IsSelected);

        public virtual BackLinkModel BackLink
            => new() { RouteName = string.Empty };
    }
}