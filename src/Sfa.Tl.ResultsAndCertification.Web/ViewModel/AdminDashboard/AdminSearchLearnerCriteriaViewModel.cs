namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerCriteriaViewModel
    {
        public AdminSearchLearnerFiltersViewModel SearchLearnerFilters { get; set; }

        public string Provider { get; set; }

        public string SearchKey { get; set; }

        public int? PageNumber { get; set; }

        public bool IsSearchKeyApplied { get; set; }
    }
}