using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class CancelRegistrationViewModel
    {
        public long Uln { get; set; }

        public int ProfileId { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ErrorResource.CancelRegistration), ErrorMessageResourceName = "Validation_Message")]
        public bool? CancelRegistration { get; set; }
        
        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.RegistrationDetails,
                    RouteAttributes = new Dictionary<string, string> { { "profileId", ProfileId.ToString() } }
                };
            }
        }
    }
}
