using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class WithdrawnConfirmationViewModel
    {
        public bool? IsWithdrawnConfirmed { get; set; }
        public bool? IsPendingWithdrawl { get; set; }
        public RegistrationPathwayStatus RegistrationPathwayStatus { get; set; }
        public string AwardingOrganisationName { get; set; }
        public int ProfileId { get; set; }
        public string LearnerName { get; set; }
        public long ProviderUkprn { get; set; }

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