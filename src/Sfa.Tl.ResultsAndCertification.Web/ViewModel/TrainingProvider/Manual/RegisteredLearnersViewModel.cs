using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class RegisteredLearnersViewModel
    {
        public SearchLearnerFiltersViewModel SearchLearnerFilters { get; set; }

        public SearchLearnerDetailsListViewModel SearchLearnerDetailsList { get; set; }

        public string SearchKey { get; set; }

        public string SortOrder { get; set; }

        public BreadcrumbModel Breadcrumb => new()
        {
            BreadcrumbItems = new List<BreadcrumbItem> { new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home } }
        };
    }

    public class SearchLearnerFiltersViewModel
    {
        public IList<FilterLookupData> AcademicYears { get; set; }
        public IList<FilterLookupData> Tlevels { get; set; }
        public IList<FilterLookupData> Status { get; set; }
    }
}
