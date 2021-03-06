﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressSelectViewModel
    {
        public AddAddressSelectViewModel()
        {
            AddressSelectList = new List<SelectListItem>();
        }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddAddressSelect), ErrorMessageResourceName = "Validation_Select_Your_Address_From_The_List")]
        public long? SelectedAddressUprn { get; set; }

        public string DepartmentName { get; set; }

        public string Postcode { get; set; }

        public IList<SelectListItem> AddressSelectList { get; set; }

        public AddressViewModel SelectedAddress { get; set; }

        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.AddAddressPostcode
        };
    }
}