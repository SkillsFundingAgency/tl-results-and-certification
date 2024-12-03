using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using AdminAddProviderContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminProvider.AdminAddProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider
{
    public class AdminAddProviderViewModel
    {
        [Required(ErrorMessageResourceType = typeof(AdminAddProviderContent), ErrorMessageResourceName = "Validation_Message_Ukprn_Required")]
        [RegularExpression(@"^\d{8}$", ErrorMessageResourceType = typeof(AdminAddProviderContent), ErrorMessageResourceName = "Validation_Message_Ukprn_Only_Numbers")]
        public string UkPrn { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminAddProviderContent), ErrorMessageResourceName = "Validation_Message_Name_Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(AdminAddProviderContent), ErrorMessageResourceName = "Validation_Message_Name_Char_Limit_Exceed")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminAddProviderContent), ErrorMessageResourceName = "Validation_Message_DisplayName_Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(AdminAddProviderContent), ErrorMessageResourceName = "Validation_Message_DisplayName_Char_Limit_Exceed")]
        public string DisplayName { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminFindProvider
        };
    }
}