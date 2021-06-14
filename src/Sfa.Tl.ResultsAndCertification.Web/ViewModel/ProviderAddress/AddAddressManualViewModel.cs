using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressManualViewModel : AddAddressBaseViewModel
    {
        public bool IsFromSelectAddress { get; set; }

        public bool IsFromAddressMissing { get; set; }

        public virtual BackLinkModel BackLink
        {
            get
            {
                var routeAttributes = new Dictionary<string, string>();
                if (!IsFromSelectAddress)
                    routeAttributes.Add(Constants.ShowPostcode, "false");
                if (IsFromAddressMissing)
                    routeAttributes.Add(Constants.IsAddressMissing, "true");

                return new BackLinkModel
                {
                    RouteName = IsFromSelectAddress ? RouteConstants.AddAddressSelect : RouteConstants.AddAddressPostcode,
                    RouteAttributes = routeAttributes
                };
            }
        }
    }
}
