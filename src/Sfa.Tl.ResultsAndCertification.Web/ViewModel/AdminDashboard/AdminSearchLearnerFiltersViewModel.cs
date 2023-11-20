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

        public IList<string> SelectedFilters
        {
            get
            {
                var selectedFilters = new List<string>();

                if (!AwardingOrganisations.IsNullOrEmpty())
                {
                    selectedFilters.AddRange(AwardingOrganisations.Where(a => a.IsSelected).Select(a => a.Name));
                }

                if (!AcademicYears.IsNullOrEmpty())
                {
                    selectedFilters.AddRange(AcademicYears.Where(a => a.IsSelected).Select(a => a.Name));
                }

                return selectedFilters;
            }
        }
    }
}