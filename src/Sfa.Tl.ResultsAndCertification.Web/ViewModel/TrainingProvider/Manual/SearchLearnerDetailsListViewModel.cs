using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerDetailsListViewModel
    {
        public SearchLearnerDetailsListViewModel()
        {
            SearchLearnerDetailsList = new List<SearchLearnerDetailsViewModel>();
        }

        public IList<SearchLearnerDetailsViewModel> SearchLearnerDetailsList;
        public int TotalRecords { get; set; }
    }
}
