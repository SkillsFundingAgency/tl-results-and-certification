using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddPostalAddressManualViewModel : AddAddressBaseViewModel
    {
        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.AddAddressPostcode
        };
    }
}
