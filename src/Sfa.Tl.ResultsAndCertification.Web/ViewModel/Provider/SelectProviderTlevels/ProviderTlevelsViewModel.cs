using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Provider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels
{
    public class ProviderTlevelsViewModel
    {
        public int ProviderId { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.SelectProviderTlevels), ErrorMessageResourceName = "Select_Tlevel_Validation_Message")]
        public bool? HasTlevelSelected => (Tlevels.Any(x => x.IsSelected) == true) ? true : (bool?)null;
        public bool HasMoreThanOneTlevel => Tlevels?.Count() > 1;
        public IList<ProviderTlevelViewModel> Tlevels { get; set; }
        public bool IsAddTlevel { get; set; }
    }
}
