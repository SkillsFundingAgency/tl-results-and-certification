using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration
{
    public class SearchRegistrationFiltersViewModel
    {
        /// <summary>
        /// This property is automatically populated by the script displaying the providers selection list.
        /// </summary>
        public string Search { get; set; } = string.Empty;

        public int? SelectedProviderId { get; set; }

        public IList<FilterLookupData> AcademicYears { get; set; } = new List<FilterLookupData>();

        public bool IsApplyFiltersSelected
            => !string.IsNullOrWhiteSpace(Search)
            || SelectedProviderId.HasValue && SelectedProviderId.Value > 0
            || !AcademicYears.IsNullOrEmpty() && AcademicYears.Any(p => p.IsSelected);
    }
}
