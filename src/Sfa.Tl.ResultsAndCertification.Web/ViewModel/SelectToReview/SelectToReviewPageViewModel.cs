using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel;

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

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ErrorResource.SelectToReview), ErrorMessageResourceName = "More_SelectTlevel_Validation_Error_Message")]
        public int SelectedPathwayId { get; set; }
    }
}
