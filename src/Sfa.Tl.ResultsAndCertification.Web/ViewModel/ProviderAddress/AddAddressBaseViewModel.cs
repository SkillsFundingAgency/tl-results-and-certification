using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public abstract class AddAddressBaseViewModel
    {
        public string DepartmentName { get; set; }
        public string OrganisationName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddPostalAddressManual), ErrorMessageResourceName = "Validation_Enter_Building_And_Street")]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddPostalAddressManual), ErrorMessageResourceName = "Validation_Enter_Town_Or_City")]
        public string Town { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddPostalAddressManual), ErrorMessageResourceName = "Validation_Enter_Postcode")]
        [RegularExpression(Constants.PostcodeValidationRegex, ErrorMessageResourceType = typeof(ErrorResource.AddPostalAddressManual), ErrorMessageResourceName = "Validation_Enter_Valid_Postcode")]
        public string Postcode { get; set; }
    }
}