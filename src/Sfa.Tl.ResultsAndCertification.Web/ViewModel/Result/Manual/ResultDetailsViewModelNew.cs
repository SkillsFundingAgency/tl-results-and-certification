using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.Linq;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ResultDetailsViewModelNew : ResultsBaseViewModel
    {
        public ResultDetailsViewModelNew()
        {
            // Base Profile Summary
            UlnLabel = ResultDetailsContent.Title_Uln_Text;
            LearnerNameLabel = ResultDetailsContent.Title_Name_Text;
            DateofBirthLabel = ResultDetailsContent.Title_DateofBirth_Text;
            ProviderUkprnLabel = ResultDetailsContent.Title_Provider_Ukprn_Text;
            ProviderNameLabel = ResultDetailsContent.Title_Provider_Name_Text;
            TlevelTitleLabel = ResultDetailsContent.Title_TLevel_Text;

            CoreComponentExams = new List<ComponentExamViewModel>();
        }

        // Core Component
        public string CoreComponentDisplayName { get; set; }
        public IList<ComponentExamViewModel> CoreComponentExams { get; set; }

        // Specialism Components
        public IList<SpecialismComponentViewModel> SpecialismComponents { get; set; }

        public bool IsPathwayAssessmentEntryRegistered { get { return CoreComponentExams.Any(x => x.AssessmentId > 0); } }

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