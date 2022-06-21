using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
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
        public PagerViewModel PagerInfo { get; set; }
        public int TotalRecords { get; set; }
    }
}
