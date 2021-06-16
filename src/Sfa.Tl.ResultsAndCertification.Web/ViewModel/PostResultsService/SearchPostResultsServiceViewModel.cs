using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class SearchPostResultsServiceViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SearchPostResultsService), ErrorMessageResourceName = "Uln_Required_Validation_Message")]
        [RegularExpression(Constants.UlnValidationRegex, ErrorMessageResourceType = typeof(ErrorResource.SearchPostResultsService), ErrorMessageResourceName = "Uln_Not_Valid_Validation_Message")]
        public string SearchUln { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Reviews_And_Appeals, RouteName = RouteConstants.StartReviewAndAppeals },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Learner }
                    }
                };
            }
        }
    }
}
