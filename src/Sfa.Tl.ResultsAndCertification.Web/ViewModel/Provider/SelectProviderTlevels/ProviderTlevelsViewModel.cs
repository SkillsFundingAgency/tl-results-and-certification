using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels
{
    public class ProviderTlevelsViewModel
    {
        public int ProviderId { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }

        [Required(ErrorMessage = "Select at least one T Level")]
        public bool? HasTlevelSelected => (Tlevels.Any(x => x.IsSelected) == true) ? true : (bool?)null;
        public IList<SelectProviderTlevelViewModel> Tlevels { get; set; }
    }
}
