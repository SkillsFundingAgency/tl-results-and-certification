using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressSelectViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddAddressSelect), ErrorMessageResourceName = "Validation_Select_Your_Address_From_The_List")]
        public int? SelectedAddressUdprn { get; set; }

        public string DepartmentName { get; set; }

        public int AddressCount { get; set; }

        public IList<SelectListItem> AddressSelectList { get; set; }

        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.AddAddressPostcode
        };
    }
}