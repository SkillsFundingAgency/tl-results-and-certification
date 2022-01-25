using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using System.Collections.Generic;
using System.Linq;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ResultDetailsViewModel : ResultsBaseViewModel
    {
        public ResultDetailsViewModel()
        {
            // Base Profile Summary
            UlnLabel = ResultDetailsContent.Title_Uln_Text;
            DateofBirthLabel = ResultDetailsContent.Title_DateofBirth_Text;
            ProviderUkprnLabel = ResultDetailsContent.Title_Provider_Ukprn_Text;
            ProviderNameLabel = ResultDetailsContent.Title_Provider_Name_Text;
            TlevelTitleLabel = ResultDetailsContent.Title_TLevel_Text;

            CoreComponentExams = new List<ComponentExamViewModel>();
            SpecialismComponents = new List<SpecialismComponentViewModel>();
        }

        public int ProfileId { get; set; }

        // Core Component
        public string CoreComponentDisplayName { get; set; }
        public bool IsCoreAssessmentEntryRegistered { get { return CoreComponentExams.Any(x => x.AssessmentId > 0); } }
        public IList<ComponentExamViewModel> CoreComponentExams { get; set; }

        // Specialism Components
        public IList<SpecialismComponentViewModel> SpecialismComponents { get; set; }
        public IList<SpecialismComponentViewModel> RenderSpecialismComponents
        {
            get
            {
                // When hasMoreThanOneSpecialism && NoneHasAssessmentEntries Then showMultiSpecialismsTogether
                var showMultiSpecialismsTogether = SpecialismComponents.Count > 1 && SpecialismComponents.All(x => !x.IsSpecialismAssessmentEntryRegistered);
                if (showMultiSpecialismsTogether)
                {
                    return new List<SpecialismComponentViewModel>
                    {
                        new SpecialismComponentViewModel { SpecialismComponentDisplayName = string.Join(Constants.AndSeperator, SpecialismComponents.Select(x => x.SpecialismComponentDisplayName)) } 
                    };
                }

                return SpecialismComponents;
            }
        }

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
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Result_Dashboard, RouteName = RouteConstants.ResultsDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Results, RouteName = RouteConstants.SearchResults }
                    }
                };
            }
        }
    }
}