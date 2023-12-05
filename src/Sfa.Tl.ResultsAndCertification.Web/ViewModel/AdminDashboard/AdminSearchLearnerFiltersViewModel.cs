using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerFiltersViewModel
    {
        public string Provider { get; set; } = string.Empty;

        public IList<FilterLookupData> AwardingOrganisations { get; set; }

        public IList<FilterLookupData> AcademicYears { get; set; }

        public bool IsApplyFiltersSelected { get; set; }
    }
}
