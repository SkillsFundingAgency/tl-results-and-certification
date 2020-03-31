using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels
{
    public class ProviderViewModel
    {
        public ProviderViewModel()
        {
            Tlevels = new List<TlevelViewModel>();
        }

        public int ProviderId { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }
        public bool IsNavigatedFromFindProvider { get; set; }
        public bool AnyTlevelsAvailable { get { return Tlevels.Any(x => x.TqProviderId.HasValue); } }
        public bool ShowAnotherTlevelButton { get { return Tlevels.Any(x => !x.TqProviderId.HasValue); } }
        public IEnumerable<TlevelViewModel> ProviderTlevels { get { return Tlevels.Where(x => x.TqProviderId.HasValue); } }
        public IList<TlevelViewModel> Tlevels { get; set; }
    }
}
