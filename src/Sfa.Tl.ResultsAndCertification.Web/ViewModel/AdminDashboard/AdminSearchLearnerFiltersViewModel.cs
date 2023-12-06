using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerFiltersViewModel
    {
        public string Provider { get; set; } = string.Empty;

        public IList<FilterLookupData> AwardingOrganisations { get; set; } = new List<FilterLookupData>();

        public IList<FilterLookupData> AcademicYears { get; set; } = new List<FilterLookupData>();

        public bool IsApplyFiltersSelected
            => !string.IsNullOrWhiteSpace(Provider)
            || (!AwardingOrganisations.IsNullOrEmpty() && AwardingOrganisations.Any(p => p.IsSelected))
            || (!AcademicYears.IsNullOrEmpty() && AcademicYears.Any(p => p.IsSelected));
    }
}
