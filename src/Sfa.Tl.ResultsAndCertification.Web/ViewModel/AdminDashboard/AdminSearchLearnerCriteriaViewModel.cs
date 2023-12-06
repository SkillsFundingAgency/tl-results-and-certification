namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerCriteriaViewModel
    {
        public AdminSearchLearnerFiltersViewModel SearchLearnerFilters { get; set; }

        public string SearchKey { get; set; }

        public int? PageNumber { get; set; }

        public bool IsSearchKeyApplied => !string.IsNullOrWhiteSpace(SearchKey);

        public bool AreFiltersApplied => SearchLearnerFilters?.IsApplyFiltersSelected == true;
    }
}