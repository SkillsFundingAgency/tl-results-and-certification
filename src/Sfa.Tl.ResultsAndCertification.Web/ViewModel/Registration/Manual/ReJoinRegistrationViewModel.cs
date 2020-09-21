using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReJoinRegistrationViewModel
    {
        public int ProfileId { get; set; }

        public long Uln { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.ReJoinRegistration), ErrorMessageResourceName = "Select_ReJoin_Validation_Message")]
        public bool? CanReJoin { get; set; }

        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.AmendWithdrawRegistration, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.ChangeStatusId, ((int)RegistrationChangeStatus.ReJoin).ToString() } } };
    }
}
