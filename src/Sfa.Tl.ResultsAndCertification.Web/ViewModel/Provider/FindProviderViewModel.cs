using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Provider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider
{
    public class FindProviderViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.FindProvider), ErrorMessageResourceName = "ProviderName_Required_Validation_Message")]
        [StringLength(400, ErrorMessageResourceType = typeof(ErrorResource.FindProvider), ErrorMessageResourceName = "ProviderName_Char_Limit_Exceed_Validation_Message")]
        public string Search { get; set; }

        public bool ShowViewProvidersLink { get; set; }

        public int SelectedProviderId { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Provider_Find_Provider }
                    }
                };
            }
        }
    }
}
