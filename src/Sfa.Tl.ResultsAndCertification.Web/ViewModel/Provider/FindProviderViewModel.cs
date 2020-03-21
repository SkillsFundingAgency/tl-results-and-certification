using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider
{
    public class FindProviderViewModel
    {
        [Required(ErrorMessage = "Enter a provider’s name")] //TODO: ContentMessage
        [StringLength(400, ErrorMessage = "The character limit exceeded")]
        public string Search { get; set; }
    }
}
