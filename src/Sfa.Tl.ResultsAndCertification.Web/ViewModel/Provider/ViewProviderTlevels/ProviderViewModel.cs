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

        //public int ProviderId { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }

        //public bool? HasTlevelSelected => (Tlevels.Any(x => x.IsSelected) == true) ? true : (bool?)null;
        //public bool HasMoreThanOneTlevel => Tlevels?.Count() > 1;

        public bool AnyTlevelSetupCompleted { get { return Tlevels.Any(x => x.TqProviderId != null); } }
        public bool ShowAnotherTlevelButton { get { return Tlevels.Any(x => x.TqProviderId == null); } }

        public IList<TlevelViewModel> Tlevels { get; set; }
    }
}
