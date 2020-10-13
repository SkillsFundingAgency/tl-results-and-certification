using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class RejoinRegistrationViewModel
    {
        public int ProfileId { get; set; }

        public long Uln { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.RejoinRegistration), ErrorMessageResourceName = "Select_Rejoin_Validation_Message")]
        public bool? CanRejoin { get; set; }

        public bool IsFromCoreDenialPage { get; set; }

        public bool IsChangeMode { get; set; }

        public BackLinkModel BackLink => new BackLinkModel 
        {
            RouteName = IsFromCoreDenialPage ? RouteConstants.ReregisterCannotSelectSameCore : RouteConstants.AmendWithdrawRegistration,
            RouteAttributes = IsFromCoreDenialPage 
                            ? IsChangeMode ? new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.IsChangeMode, "true" } } : new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } 
                            : new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.ChangeStatusId, ((int)RegistrationChangeStatus.Rejoin).ToString() } } 
        };
    }
}
