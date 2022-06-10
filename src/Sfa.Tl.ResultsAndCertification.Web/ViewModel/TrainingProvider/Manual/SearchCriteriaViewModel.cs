namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchCriteriaViewModel
    {
        public SearchLearnerFiltersViewModel SearchLearnerFilters { get; set; }

        public string SearchKey { get; set; }        

        public int AcademicYear { get; set; }
        public int? PageNumber { get; set; }
    }
}
