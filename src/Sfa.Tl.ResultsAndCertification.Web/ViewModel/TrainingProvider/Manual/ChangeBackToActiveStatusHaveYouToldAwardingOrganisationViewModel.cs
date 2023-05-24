using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.ChangeBackToActiveStatusHaveYouToldAwardingOrganisation), ErrorMessageResourceName = "Validation_Message")]
        public bool? HaveYouToldAwardingOrganisation { get; set; }
        public int ProfileId { get; set; }
        public string AwardingOrganisationName { get; set; }
        public long AwardingOrganisationUkprn { get; set; }
        public string LearnerName { get; set; }
        public int AcademicYear { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Registered_Learners, RouteName = RouteConstants.SearchLearnerDetails, RouteAttributes = new Dictionary<string, string> { { Constants.AcademicYear, AcademicYear.ToString() } } },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Learners_Record, RouteName = RouteConstants.LearnerRecordDetails, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } }
                    }
                };
            }
        }
    }
}