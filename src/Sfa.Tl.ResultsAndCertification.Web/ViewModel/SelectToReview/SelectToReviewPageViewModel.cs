using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview
{
    public class SelectToReviewPageViewModel
    {
        public SelectToReviewPageViewModel()
        {
            TlevelsToReview = new List<TlevelToReviewViewModel>();
        }

        public bool IsOnlyOneTlevelReviewPending { get { return TlevelsToReview.Count() == 1; } }

        public IEnumerable<TlevelToReviewViewModel> TlevelsToReview { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ErrorResource.SelectToReview), ErrorMessageResourceName = "Validation_Error_Message")]
        public int SelectedPathwayId { get; set; }

        public BreadcrumbModel BreadCrumb 
        { 
            get 
            { 
                return new BreadcrumbModel 
                { 
                    BreadcrumbItems = new List<BreadcrumbItem> 
                    { 
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Tlevels_Dashboard, RouteName = RouteConstants.TlevelsDashboard },                        
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Tlevel_Review_Select }
                    } 
                }; 
            } 
        }
    }
}
