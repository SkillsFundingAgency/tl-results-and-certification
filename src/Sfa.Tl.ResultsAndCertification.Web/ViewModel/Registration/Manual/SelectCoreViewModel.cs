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

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.AddRegistrationProvider,
                };
            }
        }
    }
}
