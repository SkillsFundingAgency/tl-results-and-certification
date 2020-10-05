using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregisterCannotSelectSameCoreViewModel
    {
        public int ProfileId { get; set; }

        public bool IsFromCoreDenialPage => true;

        public bool IsChangeMode { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.ReregisterCore,
                    RouteAttributes = IsChangeMode ? new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.IsChangeMode, "true" } } : new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                };
            }
        }
    }
}
