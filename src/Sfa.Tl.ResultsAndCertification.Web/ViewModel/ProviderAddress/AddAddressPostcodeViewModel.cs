using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressPostcodeViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddAddressPostcode), ErrorMessageResourceName = "Validation_Enter_Postcode")]
        [RegularExpression("[A-Za-z]{1,2}[0-9Rr][0-9A-Za-z]?\\s?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}", ErrorMessageResourceType = typeof(ErrorResource.AddAddressPostcode), ErrorMessageResourceName = "Validation_Enter_Valid_Postcode")]
        public string Postcode { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.ManagePostalAddress
        };
    }
}