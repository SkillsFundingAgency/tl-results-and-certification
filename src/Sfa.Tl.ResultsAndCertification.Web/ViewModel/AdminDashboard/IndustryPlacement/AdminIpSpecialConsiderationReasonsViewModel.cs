using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement
{
    public class AdminIpSpecialConsiderationReasonsViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public IList<IpLookupDataViewModel> ReasonsList { get; set; } = new List<IpLookupDataViewModel>();

        [RequiredTrue(ErrorMessageResourceType = typeof(AdminIndustryPlacementSpecialConsiderationReasons), ErrorMessageResourceName = "Validation_Message_Select_One_Or_More_Reasons")]
        public bool IsReasonSelected
            => !ReasonsList.IsNullOrEmpty() && ReasonsList.Any(p => p.IsSelected);

        public virtual BackLinkModel BackLink
            => new() { RouteName = RouteConstants.AdminIndustryPlacementSpecialConsiderationHours };
    }
}