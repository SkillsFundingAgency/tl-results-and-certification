using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Provider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminSearchLearnerFiltersViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.FindProvider), ErrorMessageResourceName = "ProviderName_Required_Validation_Message")]
        [StringLength(400, ErrorMessageResourceType = typeof(ErrorResource.FindProvider), ErrorMessageResourceName = "ProviderName_Char_Limit_Exceed_Validation_Message")]
        public string Provider { get; set; }

        public IList<FilterLookupData> AwardingOrganisations { get; set; } = new List<FilterLookupData>();

        public IList<FilterLookupData> AcademicYears { get; set; } = new List<FilterLookupData>();

        public bool IsApplyFiltersSelected
            => !string.IsNullOrWhiteSpace(Provider)
            || (!AwardingOrganisations.IsNullOrEmpty() && AwardingOrganisations.Any(p => p.IsSelected))
            || (!AcademicYears.IsNullOrEmpty() && AcademicYears.Any(p => p.IsSelected));
    }
}
