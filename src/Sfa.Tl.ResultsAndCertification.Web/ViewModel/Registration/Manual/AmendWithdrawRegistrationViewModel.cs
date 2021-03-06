﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class AmendWithdrawRegistrationViewModel
    {
        public int ProfileId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AmendWithdrawRegistration), ErrorMessageResourceName = "Select_Active_Options_Validation_Message")]
        public RegistrationChangeStatus? ChangeStatus { get; set; }

        public int? ChangeStatusId { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.RegistrationDetails,
                    RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                };
            }
        }

        public void SetChangeStatus()
        {
            ChangeStatus = ChangeStatusOption;
        }

        private bool IsValidChangeStatus => EnumExtensions.IsValidValue<RegistrationChangeStatus>(ChangeStatusId);
        private RegistrationChangeStatus? ChangeStatusOption => IsValidChangeStatus ? (RegistrationChangeStatus?)ChangeStatusId : null;
    }
}
