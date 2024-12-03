using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdminEditProviderContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminProvider.AdminEditProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider
{
    public class AdminEditProviderViewModel
    {
        public int ProviderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminEditProviderContent), ErrorMessageResourceName = "Validation_Message_Ukprn_Required")]
        [RegularExpression(@"^\d{8}$", ErrorMessageResourceType = typeof(AdminEditProviderContent), ErrorMessageResourceName = "Validation_Message_Ukprn_Only_Numbers")]
        public string UkPrn { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminEditProviderContent), ErrorMessageResourceName = "Validation_Message_Name_Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(AdminEditProviderContent), ErrorMessageResourceName = "Validation_Message_Name_Char_Limit_Exceed")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminEditProviderContent), ErrorMessageResourceName = "Validation_Message_DisplayName_Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(AdminEditProviderContent), ErrorMessageResourceName = "Validation_Message_DisplayName_Char_Limit_Exceed")]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminProviderDetails,
            RouteAttributes = new Dictionary<string, string> { { "providerId", ProviderId.ToString() } }
        };
    }
}