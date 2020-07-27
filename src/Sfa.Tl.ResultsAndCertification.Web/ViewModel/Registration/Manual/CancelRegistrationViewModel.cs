using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class CancelRegistrationViewModel
    {
        public long Uln { get; set; }

        public int TqRegistrationProfileId { get; set; }
        
        [Required(ErrorMessage = "Select yes if you want to cancel this registration")]
        public bool? CancelRegistration { get; set; }
        
        public BackLinkModel BackLink
        {
            get
            {
                // TODO: Check the links. 
                return new BackLinkModel
                {
                    RouteName = "Search-for-registration-registration-details",
                    RouteAttributes = new Dictionary<string, string> { { "tqRegistrationProfileId", TqRegistrationProfileId.ToString() } }
                };
            }
        }
    }
}
