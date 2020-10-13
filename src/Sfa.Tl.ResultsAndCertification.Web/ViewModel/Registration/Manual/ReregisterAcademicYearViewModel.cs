using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregisterAcademicYearViewModel : SelectAcademicYearViewModel
    {
        public int ProfileId { get; set; }

        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = IsChangeMode ? RouteConstants.ReregisterCheckAndSubmit : HasSpecialismsSelected ? RouteConstants.ReregisterSpecialisms : RouteConstants.ReregisterSpecialismQuestion,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}