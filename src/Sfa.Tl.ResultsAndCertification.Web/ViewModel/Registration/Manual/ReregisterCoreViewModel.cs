using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregisterCoreViewModel : SelectCoreViewModel
    {
        public int ProfileId { get; set; }

        public string CoreCodeAtTheTimeOfWithdrawn { get; set; }

        public bool IsValidCore => !string.IsNullOrWhiteSpace(SelectedCoreCode) && !string.IsNullOrWhiteSpace(CoreCodeAtTheTimeOfWithdrawn) && !SelectedCoreCode.Equals(CoreCodeAtTheTimeOfWithdrawn, StringComparison.InvariantCultureIgnoreCase);

        public override BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.ReregisterProvider,
                    RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                };
            }
        }
    }
}
