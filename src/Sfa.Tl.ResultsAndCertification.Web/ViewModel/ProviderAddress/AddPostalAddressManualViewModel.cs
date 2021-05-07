using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddPostalAddressManualViewModel
    {
        public string Department { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddPostalAddressManual), ErrorMessageResourceName = "Validation_Enter_Building_And_Street")] 
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddPostalAddressManual), ErrorMessageResourceName = "Validation_Enter_Town_Or_City")]
        public string Town { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddPostalAddressManual), ErrorMessageResourceName = "Validation_Enter_Postcode")]
        [RegularExpression(Constants.PostcodeValidationRegex, ErrorMessageResourceType = typeof(ErrorResource.AddPostalAddressManual), ErrorMessageResourceName = "Validation_Enter_Valid_Postcode")]
        public string Postcode { get; set; }

        public bool IsFromSelectAddress { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = IsFromSelectAddress ? RouteConstants.AddAddressSelect : RouteConstants.AddAddressPostcode
        };
    }
}
