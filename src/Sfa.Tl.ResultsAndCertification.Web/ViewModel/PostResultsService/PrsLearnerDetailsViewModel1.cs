using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using System.Collections.Generic;
using System.Linq;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using PrsLearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsLearnerDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsLearnerDetailsViewModel1 : PrsBaseViewModel
    {
        public PrsLearnerDetailsViewModel1()
        {
            UlnLabel = PrsLearnerDetailsContent.Title_Uln_Text;
            LearnerNameLabel = PrsLearnerDetailsContent.Title_Name_Text;
            DateofBirthLabel = PrsLearnerDetailsContent.Title_DateofBirth_Text;
            ProviderNameLabel = PrsLearnerDetailsContent.Title_Provider_Name_Text;
            ProviderUkprnLabel = PrsLearnerDetailsContent.Title_Provider_Ukprn_Text;
            TlevelTitleLabel = PrsLearnerDetailsContent.Title_TLevel_Text;
        }

        public int ProfileId { get; set; }        

        // Core Component
        public string CoreComponentDisplayName { get; set; }
        public bool HasCoreResults { get { return PrsCoreComponentExams.Any(x => x.AssessmentId > 0 && !string.IsNullOrWhiteSpace(x.Grade)); } }
        public IList<PrsComponentExamViewModel> PrsCoreComponentExams { get; set; }

        // Specialism Components
        public IList<PrsSpecialismComponentViewModel> PrsSpecialismComponents { get; set; }

        public NotificationBannerModel SuccessBanner { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.StartPostResultsService, RouteName = RouteConstants.StartReviewsAndAppeals },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Learner, RouteName = RouteConstants.PrsSearchLearner }
                    }
                };
            }
        }
    }
}