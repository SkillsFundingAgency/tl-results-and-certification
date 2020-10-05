using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregisterProviderViewModel : SelectProviderViewModel
    {
        public int ProfileId { get; set; }

        public override BackLinkModel BackLink
        {
            get
            {
                if (IsChangeMode)
                {
                    return new BackLinkModel
                    {
                        RouteName = RouteConstants.ReregisterCheckAndSubmit,
                        RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                    };
                }

                return new BackLinkModel
                {
                    RouteName = RouteConstants.AmendWithdrawRegistration,
                    RouteAttributes = new Dictionary<string, string>
                    { 
                        { Constants.ProfileId, ProfileId.ToString() },
                        { Constants.ChangeStatusId, ((int)RegistrationChangeStatus.Reregister).ToString() }
                    }
                };
            }
        }
    }
}
