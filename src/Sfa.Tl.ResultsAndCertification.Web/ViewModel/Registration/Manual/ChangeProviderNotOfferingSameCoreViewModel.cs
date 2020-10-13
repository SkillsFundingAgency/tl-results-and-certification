using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ChangeProviderNotOfferingSameCoreViewModel
    {
        public int ProfileId { get; set; }

        public string ProviderDisplayName { get; set; }

        public string CoreDisplayName { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.ChangeRegistrationCoreQuestion,
                    RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                };
            }
        }
    }
}
