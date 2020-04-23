using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{

    public class YourTlevelsViewModel
    {
        public YourTlevelsViewModel()
        {
            ConfirmedTlevels = new List<YourTlevelViewModel>();
            QueriedTlevels = new List<YourTlevelViewModel>();
        }

        public bool IsAnyReviewPending { get; set; }
        public List<YourTlevelViewModel> ConfirmedTlevels { get; set; }
        public List<YourTlevelViewModel> QueriedTlevels { get; set; }
    }
}
