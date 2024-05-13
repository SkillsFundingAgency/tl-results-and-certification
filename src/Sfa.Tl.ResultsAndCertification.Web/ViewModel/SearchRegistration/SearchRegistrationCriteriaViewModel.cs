namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration
{
    public class SearchRegistrationCriteriaViewModel
    {
        public SearchRegistrationFiltersViewModel Filters { get; set; }

        public string SearchKey { get; set; } = string.Empty;

        public int? PageNumber { get; set; }

        public bool IsSearchKeyApplied => !string.IsNullOrWhiteSpace(SearchKey);

        public bool AreFiltersApplied => Filters?.IsApplyFiltersSelected == true;
    }
}