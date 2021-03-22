using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerRecordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SearchLearnerRecord), ErrorMessageResourceName = "Uln_Required_With_10Digit_Validation_Message")]
        [RegularExpression(@"^\d{10}$", ErrorMessageResourceType = typeof(ErrorResource.SearchLearnerRecord), ErrorMessageResourceName = "Uln_Must_Include_NumbersOnly_Validation_Message")]
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
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Manage_Learner_Records, RouteName = RouteConstants.ManageLearnerRecordsDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Learner }
                    }
                };
            }
        }
    }
}
