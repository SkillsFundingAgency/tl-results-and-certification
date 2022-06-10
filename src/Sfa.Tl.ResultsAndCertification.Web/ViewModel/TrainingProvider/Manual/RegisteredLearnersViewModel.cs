using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class RegisteredLearnersViewModel
    {
        public SearchCriteriaViewModel SearchCriteria { get; set; }
        public SearchLearnerDetailsListViewModel SearchLearnerDetailsList { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Manage_Learner_Records, RouteName = RouteConstants.SearchLearnerRecord }
                    }
                };
            }
        }

        public PaginationModel Pagination
        {
            get
            {
                return new PaginationModel
                {
                    PagerInfo = SearchLearnerDetailsList?.PagerInfo,
                    RouteName = RouteConstants.SearchLearnerDetails,
                    RouteAttributes = new Dictionary<string, string> { { Constants.AcademicYear, SearchCriteria.AcademicYear.ToString() } },
                    PaginationSummary = SearchLearnerDetails.PaginationSummary_Text
                };
            }
        }
    }
}
