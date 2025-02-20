using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using AdminFindProviderContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDownloadLearnerResults.AdminDownloadLearnerResultsFindProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults
{
    public class AdminDownloadLearnerResultsFindProviderViewModel
    {
        [Required(ErrorMessageResourceType = typeof(AdminFindProviderContent), ErrorMessageResourceName = "ProviderName_Required_Validation_Message")]
        [StringLength(400, ErrorMessageResourceType = typeof(AdminFindProviderContent), ErrorMessageResourceName = "ProviderName_Char_Limit_Exceed_Validation_Message")]
        public string Search { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new() { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.AdminHome },
                        new() { DisplayName = BreadcrumbContent.Provider_Find_Provider }
                    }
                };
            }
        }
    }
}
