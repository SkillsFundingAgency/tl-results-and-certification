using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminSearchChangeLogViewModel
    {
        public string SearchKey { get; set; }

        public int? PageNumber { get; set; }

        public IList<AdminSearchChangeLogDetailsViewModel> ChangeLogDetails { get; set; } = new List<AdminSearchChangeLogDetailsViewModel>();

        public PagerViewModel PagerInfo { get; set; }

        public int TotalRecords { get; set; }
    }
}