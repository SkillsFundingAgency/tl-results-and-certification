using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AssessmentDetailsViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }

        public string ProviderDisplayName { get; set; }
        public string PathwayDisplayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }


        public string SpecialismDisplayName { get; set; }
        public string SpecialismAssessmentSeries { get; set; }

        public RegistrationPathwayStatus PathwayStatus { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Assessment_Dashboard, RouteName = RouteConstants.AssessmentDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Assessments, RouteName = RouteConstants.SearchAssessments },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Learners_Assessment_entries }
                    }
                };
            }
        }
    }
}
