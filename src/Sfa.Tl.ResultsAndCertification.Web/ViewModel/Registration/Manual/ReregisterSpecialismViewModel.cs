using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregisterSpecialismViewModel : SelectSpecialismViewModel
    {
        public int ProfileId { get; set; }

        public override BackLinkModel BackLink => IsChangeMode
            ? new BackLinkModel
            {
                RouteName = IsChangeModeFromSpecialismQuestion ? RouteConstants.ReregisterCore : RouteConstants.ReregisterCheckAndSubmit,
                RouteAttributes = IsChangeModeFromSpecialismQuestion ?
                            new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.IsChangeMode, "true" } } :
                            new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
            }
            : new BackLinkModel
            {
                RouteName = RouteConstants.ReregisterSpecialismQuestion,
                RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
            };
    }
}
