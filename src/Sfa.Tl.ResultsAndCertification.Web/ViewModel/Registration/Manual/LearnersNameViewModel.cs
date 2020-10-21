using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class LearnersNameViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.LearnersName), ErrorMessageResourceName = "Validation_Firstname_Required")]
        [MaxLength(100, ErrorMessageResourceType = typeof(ErrorResource.LearnersName), ErrorMessageResourceName = "Validation_Firstname_Max_Length")]
        public string Firstname { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.LearnersName), ErrorMessageResourceName = "Validation_Lastname_Required")]
        [MaxLength(100, ErrorMessageResourceType = typeof(ErrorResource.LearnersName), ErrorMessageResourceName = "Validation_Lastname_Max_Length")]
        public string Lastname { get; set; }
        public bool IsChangeMode { get; set; }
        public virtual BackLinkModel BackLink => new BackLinkModel { RouteName = IsChangeMode ? RouteConstants.AddRegistrationCheckAndSubmit : RouteConstants.AddRegistrationUln };
    }
}
