using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregisterSpecialismQuestionViewModel : SpecialismQuestionViewModel
    {
        public int ProfileId { get; set; }

        public override BackLinkModel BackLink => IsChangeMode
                    ? new BackLinkModel
                    {
                        RouteName = IsChangeModeFromCore ? RouteConstants.ReregisterCore : RouteConstants.ReregisterCheckAndSubmit,
                        RouteAttributes = IsChangeModeFromCore ?
                            new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.IsChangeMode, "true" } } :
                            new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                    }
                    : new BackLinkModel
                    {
                        RouteName = RouteConstants.ReregisterCore,
                        RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, }
                    };
    }
}