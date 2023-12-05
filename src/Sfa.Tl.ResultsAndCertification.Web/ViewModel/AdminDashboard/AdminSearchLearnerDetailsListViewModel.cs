using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerDetailsListViewModel
    {
        public IList<AdminSearchLearnerDetailsViewModel> LearnerDetails { get; set; } = new List<AdminSearchLearnerDetailsViewModel>();

        public PagerViewModel PagerInfo { get; set; }

        public int TotalRecords { get; set; }
    }
}