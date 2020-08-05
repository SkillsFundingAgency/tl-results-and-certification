using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class UlnViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.UlnRegistration), ErrorMessageResourceName = "Validation_Uln_Required")]
        [RegularExpression("^((?!(0))[0-9]{10})$", ErrorMessageResourceType = typeof(ErrorResource.UlnRegistration), ErrorMessageResourceName = "Validation_Uln_Must_Be_Digits")]
        public string Uln { get; set; }
        public bool IsChangeMode { get; set; }
        public BackLinkModel BackLink => new BackLinkModel { RouteName = IsChangeMode ? RouteConstants.AddRegistrationCheckAndSubmit : RouteConstants.RegistrationDashboard };
    }
}