using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressCheckAndSubmitViewModel : AddAddressBaseViewModel
    {
        public bool IsManual { get; set; }

        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = IsManual ? RouteConstants.AddPostalAddressManul : RouteConstants.AddAddressSelect
        };
    }
}
