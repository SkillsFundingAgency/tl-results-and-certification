using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class SelectProviderViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SelectProvider), ErrorMessageResourceName = "Validation_Select_Provider_Required")]
        public string SelectedProviderId { get; set; }

        //public IList<ProviderDetailsViewModel> RegistrationProviders { get; set; }

        public IList<SelectListItem> ProvidersSelectList { get; set; }

        //public IList<SelectListItem> ProvidersSelectList => RegistrationProviders?.Select(p =>
        //    new SelectListItem
        //    {
        //        Value = p.ProviderId.ToString(),
        //        Text = p.DisplayNameWithUkprn
        //    }).ToList();
    }
}
