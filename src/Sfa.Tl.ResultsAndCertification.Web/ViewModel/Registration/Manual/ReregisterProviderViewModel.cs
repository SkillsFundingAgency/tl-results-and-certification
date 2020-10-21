using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregisterProviderViewModel : SelectProviderViewModel
    {
        public bool IsFromConfirmation { get; set; }

        public int ProfileId { get; set; }

        public override BackLinkModel BackLink => IsChangeMode
                    ? new BackLinkModel
                    {
                        RouteName = RouteConstants.ReregisterCheckAndSubmit,
                        RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                    }
                    : new BackLinkModel
                    {
                        RouteName = RouteConstants.AmendWithdrawRegistration,
                        RouteAttributes = new Dictionary<string, string>
                    {
                        { Constants.ProfileId, ProfileId.ToString() },
                        { Constants.ChangeStatusId, IsFromConfirmation ? null : ((int)RegistrationChangeStatus.Reregister).ToString() }
                    }
                    };
    }
}