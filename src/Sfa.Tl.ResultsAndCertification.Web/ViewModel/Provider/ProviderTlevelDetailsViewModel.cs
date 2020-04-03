using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider
{
    public class ProviderTlevelDetailsViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }
        public string TlevelTitle { get; set; }

        [Required(ErrorMessage = "Select yes to remove the T Level")]
        public bool? CanRemoveTlevel { get; set; }
    }
}
