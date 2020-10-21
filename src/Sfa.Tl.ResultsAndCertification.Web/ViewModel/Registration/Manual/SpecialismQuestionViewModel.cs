using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class SpecialismQuestionViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SpecialismQuestion), ErrorMessageResourceName = "Validation_Select_Yes_Required_Message")]
        public bool? HasLearnerDecidedSpecialism { get; set; }

        public bool IsChangeMode { get; set; }

        public bool IsChangeModeFromCore { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel 
        { RouteName = (IsChangeMode && !IsChangeModeFromCore) ? 
            RouteConstants.AddRegistrationCheckAndSubmit : 
            RouteConstants.AddRegistrationCore, 
            
            RouteAttributes = (IsChangeMode && IsChangeModeFromCore) ? new Dictionary<string, string> { { Constants.IsChangeMode, "true" } } : null };
    }
}
