using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressNotFoundViewModel
    {
        public string Postcode { get; set; }
        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.AddAddressPostcode };
    }
}
