using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification
{
    public class AdminFindNotificationCriteriaViewModel
    {
        public IList<FilterLookupData> ActiveFilters { get; set; }

        public bool AreFiltersApplied
            => !ActiveFilters.IsNullOrEmpty() && ActiveFilters.Any(p => p.IsSelected);

        public int? PageNumber { get; set; }
    }
}