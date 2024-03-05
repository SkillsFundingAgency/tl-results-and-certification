using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminSearchChangeLogViewModel
    {
        public AdminSearchChangeLogCriteriaViewModel SearchCriteriaViewModel { get; set; } = new AdminSearchChangeLogCriteriaViewModel();

        public IList<AdminSearchChangeLogDetailsViewModel> ChangeLogDetails { get; set; } = new List<AdminSearchChangeLogDetailsViewModel>();

        public PagerViewModel PagerInfo { get; set; }

        public int TotalRecords { get; set; }

        public void SetSearchKey(string searchKey)
        {
            SearchCriteriaViewModel ??= new AdminSearchChangeLogCriteriaViewModel();
            SearchCriteriaViewModel.SearchKey = searchKey;
            SearchCriteriaViewModel.PageNumber = 1;
        }

        public void ClearChangeLogDetails()
        {
            if (ChangeLogDetails.IsNullOrEmpty())
            {
                return;
            }

            ChangeLogDetails.Clear();
        }
    }
}