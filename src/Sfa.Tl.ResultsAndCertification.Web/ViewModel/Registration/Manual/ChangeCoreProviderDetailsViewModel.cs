
namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ChangeCoreProviderDetailsViewModel
    {
        public int ProfileId { get; set; }
        public string ProviderDisplayName { get; set; }
        public string CoreDisplayName { get; set; }
        public bool? CanChangeCore { get; set; }
    }
}