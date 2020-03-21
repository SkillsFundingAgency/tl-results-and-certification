using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels
{
    public class SelectProviderTlevelViewModel
    {
        public int TlProviderId { get; set; }
        public string ProviderDisplayName { get; set; }
        public long ProviderUkprn { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        [Required(ErrorMessage = "Select at least one T Level")]
        public bool HasTlevelSelected { get { return ProviderTlevels.Any(x => x.IsSelected); } }
        public IList<ProviderTlevelsViewModel> ProviderTlevels { get; set; }
    }
}
