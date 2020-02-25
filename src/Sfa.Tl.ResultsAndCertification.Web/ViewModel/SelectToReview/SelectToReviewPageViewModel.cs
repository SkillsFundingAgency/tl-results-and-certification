using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview
{
    public class SelectToReviewPageViewModel
    {
        public SelectToReviewPageViewModel()
        {
            TlevelsToReview = new List<TlevelToReviewViewModel>();
        }

        public bool IsOnlyOneTlevelReviewPending { get { return TlevelsToReview.Count() == 1; } }
        public bool ShowViewReviewedTlevelsLink { get; set; }

        public IEnumerable<TlevelToReviewViewModel> TlevelsToReview { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TODO: Please select T level")]
        public int SelectedPathwayId { get; set; }
    }
}
