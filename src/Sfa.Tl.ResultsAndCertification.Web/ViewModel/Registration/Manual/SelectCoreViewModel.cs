using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class SelectCoreViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SelectCore), ErrorMessageResourceName = "Validation_Select_Core_Required")]
        public string SelectedCoreCode { get; set; }
        
        public string SelectedCoreDisplayName { get; set; }

        public IList<SelectListItem> CoreSelectList { get; set; }

        public bool IsChangeMode { get; set; }

        public bool IsChangeModeFromProvider { get; set; }

        public BackLinkModel BackLink => new BackLinkModel { RouteName = (IsChangeMode && !IsChangeModeFromProvider) ? RouteConstants.AddRegistrationCheckAndSubmit : RouteConstants.AddRegistrationProvider, RouteAttributes = (IsChangeMode && IsChangeModeFromProvider) ? new Dictionary<string, string> { { Constants.IsChangeMode, "true" } } : null };
    }
}
