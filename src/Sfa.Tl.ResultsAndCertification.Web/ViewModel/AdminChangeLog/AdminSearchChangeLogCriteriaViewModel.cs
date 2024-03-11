namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminSearchChangeLogCriteriaViewModel
    {
        public string SearchKey { get; set; }

        public int? PageNumber { get; set; }

        public bool IsSearchKeyApplied 
            => !string.IsNullOrWhiteSpace(SearchKey);
    }
}