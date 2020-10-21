using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ChangeSpecialismViewModel : SelectSpecialismViewModel
    {
        public int ProfileId { get; set; }
        public string CoreCode { get; set; }
        public IList<string> SpecialismCodes { get; set; }

        public override BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = SpecialismCodes?.Count > 0 ? RouteConstants.ChangeRegistrationSpecialismQuestion : RouteConstants.RegistrationDetails,
                    RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                };
            }
        }
    }
}
