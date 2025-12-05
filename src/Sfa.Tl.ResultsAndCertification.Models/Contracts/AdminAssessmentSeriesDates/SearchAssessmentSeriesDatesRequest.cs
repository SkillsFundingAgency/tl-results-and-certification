using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates
{
    public class SearchAssessmentSeriesDatesRequest
    {
        public List<int> SelectedFilters { get; set; } = new List<int>();
    }
}
