﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressPostcodeViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.AddAddressPostcode), ErrorMessageResourceName = "Validation_Enter_Postcode")]
        [RegularExpression(Constants.PostcodeValidationRegex, ErrorMessageResourceType = typeof(ErrorResource.AddPostalAddressManual), ErrorMessageResourceName = "Validation_Enter_Valid_Postcode")]
        public string Postcode { get; set; }

        public bool IsFromAddressMissing { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = IsFromAddressMissing ? RouteConstants.PostalAddressMissing : RouteConstants.ManagePostalAddress
        };
    }
}