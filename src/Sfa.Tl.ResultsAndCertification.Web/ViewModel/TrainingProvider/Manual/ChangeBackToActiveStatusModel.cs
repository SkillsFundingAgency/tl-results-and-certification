using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class ChangeBackToActiveStatusModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.ChangeBackToActiveStatus), ErrorMessageResourceName = "Validation_Message")]
        public bool? ChangeBackToActive { get; set; }
        public int ProfileId { get; set; }
        public string LearnerName { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Learner, RouteName = RouteConstants.SearchLearnerRecord },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Learners_Record, RouteName = RouteConstants.LearnerRecordDetails, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } }
                    }
                };
            }
        }

    }
}