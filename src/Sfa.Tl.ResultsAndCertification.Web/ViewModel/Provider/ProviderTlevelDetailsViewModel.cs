using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Provider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider
{
    public class ProviderTlevelDetailsViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }
        public string TlevelTitle { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.RemoveProviderTlevel), ErrorMessageResourceName = "Select_RemoveProviderTlevel_Validation_Message")]
        public bool? CanRemoveTlevel { get; set; }
    }
}
