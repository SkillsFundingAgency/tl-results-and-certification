using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddPostalAddressManualViewModel : AddAddressBaseViewModel
    {
        public bool IsFromSelectAddress { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = IsFromSelectAddress ? RouteConstants.AddAddressSelect : RouteConstants.AddAddressPostcode
        };
    }
}
